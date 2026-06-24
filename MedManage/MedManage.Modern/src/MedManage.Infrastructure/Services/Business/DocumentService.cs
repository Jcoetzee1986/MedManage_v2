using MedManage.Core.DTOs.Document;
using MedManage.Core.Interfaces.Services;
using MedManage.Core.Interfaces;
using MedManage.Core.Entities;
using AutoMapper;
using SkiaSharp;

namespace MedManage.Infrastructure.Services.Business;

/// <summary>
/// Document management service with file storage and thumbnail generation.
/// Stores files on disk and tracks metadata via the CaseLinkedFile entity.
/// </summary>
public class DocumentService : IDocumentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly string _uploadsRoot;
    private readonly string _thumbnailsRoot;

    private static readonly HashSet<string> ImageExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".png", ".jpg", ".jpeg", ".gif", ".bmp", ".webp"
    };

    private const int ThumbnailMaxWidth = 200;
    private const int ThumbnailMaxHeight = 200;

    public DocumentService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _uploadsRoot = Path.Combine(Directory.GetCurrentDirectory(), "uploads", "documents");
        _thumbnailsRoot = Path.Combine(Directory.GetCurrentDirectory(), "uploads", "thumbnails");
        Directory.CreateDirectory(_uploadsRoot);
        Directory.CreateDirectory(_thumbnailsRoot);
    }

    public async Task<IEnumerable<DocumentDto>> GetByEntityAsync(string entityType, int entityId)
    {
        IEnumerable<CaseLinkedFile> files;

        if (entityType.Equals("case", StringComparison.OrdinalIgnoreCase))
        {
            files = await _unitOfWork.CaseLinkedFiles.FindAsync(f => f.CaseId == entityId);
        }
        else if (entityType.Equals("member", StringComparison.OrdinalIgnoreCase))
        {
            files = await _unitOfWork.CaseLinkedFiles.FindAsync(f => f.MemberId == entityId);
        }
        else
        {
            return Enumerable.Empty<DocumentDto>();
        }

        return files
            .OrderByDescending(f => f.DateAdded)
            .Select(f => MapToDto(f));
    }

    public async Task<DocumentDto?> GetByIdAsync(int id)
    {
        var file = await _unitOfWork.CaseLinkedFiles.GetByIdAsync(id);
        return file == null ? null : MapToDto(file);
    }

    public async Task<DocumentDto> UploadAsync(Stream fileStream, string fileName, string contentType, long fileSize, string entityType, int entityId, string? userId)
    {
        // Determine storage path
        var storageName = $"{Guid.NewGuid()}{Path.GetExtension(fileName)}";
        var subFolder = Path.Combine(entityType.ToLowerInvariant(), entityId.ToString());
        var fullDir = Path.Combine(_uploadsRoot, subFolder);
        Directory.CreateDirectory(fullDir);
        var fullPath = Path.Combine(fullDir, storageName);

        // Write file to disk
        using (var output = new FileStream(fullPath, FileMode.Create))
        {
            await fileStream.CopyToAsync(output);
        }

        // Generate thumbnail if image
        var isImage = IsImageFile(fileName);
        if (isImage)
        {
            await GenerateThumbnailAsync(fullPath, storageName);
        }

        // Create DB record
        var entity = new CaseLinkedFile
        {
            CaseId = entityType.Equals("case", StringComparison.OrdinalIgnoreCase) ? entityId : null,
            MemberId = entityType.Equals("member", StringComparison.OrdinalIgnoreCase) ? entityId : null,
            FileName = fileName,
            FileType = Path.GetExtension(fileName)?.TrimStart('.'),
            FilePath = fullPath,
            DateAdded = DateTime.UtcNow,
            DateInserted = DateTime.UtcNow,
            UserID = userId
        };

        await _unitOfWork.CaseLinkedFiles.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(entity, fileSize);
    }

    public async Task<(Stream stream, string contentType, string fileName)?> DownloadAsync(int id)
    {
        var file = await _unitOfWork.CaseLinkedFiles.GetByIdAsync(id);
        if (file == null || string.IsNullOrEmpty(file.FilePath) || !File.Exists(file.FilePath))
            return null;

        var stream = new FileStream(file.FilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        var contentType = GetContentType(file.FileName ?? "file");
        return (stream, contentType, file.FileName ?? "download");
    }

    public async Task<(Stream stream, string contentType)?> GetThumbnailAsync(int id)
    {
        var file = await _unitOfWork.CaseLinkedFiles.GetByIdAsync(id);
        if (file == null || !IsImageFile(file.FileName))
            return null;

        // Derive thumbnail path from original file path
        var storageName = Path.GetFileName(file.FilePath);
        var thumbnailPath = Path.Combine(_thumbnailsRoot, storageName ?? "");

        if (!File.Exists(thumbnailPath))
        {
            // Generate thumbnail on-the-fly if missing
            if (!string.IsNullOrEmpty(file.FilePath) && File.Exists(file.FilePath))
            {
                await GenerateThumbnailAsync(file.FilePath, storageName!);
            }
            else
            {
                return null;
            }
        }

        if (!File.Exists(thumbnailPath))
            return null;

        var stream = new FileStream(thumbnailPath, FileMode.Open, FileAccess.Read, FileShare.Read);
        return (stream, "image/png");
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var file = await _unitOfWork.CaseLinkedFiles.GetByIdAsync(id);
        if (file == null)
            return false;

        // Soft delete in DB
        file.DateDeleted = DateTime.UtcNow;
        await _unitOfWork.CaseLinkedFiles.UpdateAsync(file);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    private Task GenerateThumbnailAsync(string sourcePath, string storageName)
    {
        return Task.Run(() =>
        {
            try
            {
                var thumbnailPath = Path.Combine(_thumbnailsRoot, storageName);
                using var inputStream = File.OpenRead(sourcePath);
                using var original = SKBitmap.Decode(inputStream);

                if (original == null)
                    return;

                // Calculate proportional size
                var ratioX = (double)ThumbnailMaxWidth / original.Width;
                var ratioY = (double)ThumbnailMaxHeight / original.Height;
                var ratio = Math.Min(ratioX, ratioY);

                var newWidth = (int)(original.Width * ratio);
                var newHeight = (int)(original.Height * ratio);

                using var resized = original.Resize(new SKImageInfo(newWidth, newHeight), SKSamplingOptions.Default);
                if (resized == null)
                    return;

                using var image = SKImage.FromBitmap(resized);
                using var data = image.Encode(SKEncodedImageFormat.Png, 80);
                using var output = File.OpenWrite(thumbnailPath);
                data.SaveTo(output);
            }
            catch
            {
                // Thumbnail generation is best-effort; don't fail the upload
            }
        });
    }

    private static bool IsImageFile(string? fileName)
    {
        if (string.IsNullOrEmpty(fileName))
            return false;
        var ext = Path.GetExtension(fileName);
        return !string.IsNullOrEmpty(ext) && ImageExtensions.Contains(ext);
    }

    private static string GetContentType(string fileName)
    {
        var ext = Path.GetExtension(fileName)?.ToLowerInvariant();
        return ext switch
        {
            ".pdf" => "application/pdf",
            ".doc" => "application/msword",
            ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            ".xls" => "application/vnd.ms-excel",
            ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            ".png" => "image/png",
            ".jpg" or ".jpeg" => "image/jpeg",
            ".gif" => "image/gif",
            ".bmp" => "image/bmp",
            ".webp" => "image/webp",
            ".txt" => "text/plain",
            ".csv" => "text/csv",
            _ => "application/octet-stream"
        };
    }

    private DocumentDto MapToDto(CaseLinkedFile file, long? fileSize = null)
    {
        var isImage = IsImageFile(file.FileName);
        var size = fileSize ?? (file.FilePath != null && File.Exists(file.FilePath)
            ? new FileInfo(file.FilePath).Length
            : 0);

        string entityType;
        int entityId;
        if (file.CaseId.HasValue)
        {
            entityType = "case";
            entityId = file.CaseId.Value;
        }
        else if (file.MemberId.HasValue)
        {
            entityType = "member";
            entityId = file.MemberId.Value;
        }
        else
        {
            entityType = "unknown";
            entityId = 0;
        }

        return new DocumentDto
        {
            Id = file.CaseLinkedFileId,
            EntityType = entityType,
            EntityId = entityId,
            FileName = file.FileName ?? "",
            FileType = file.FileType,
            FileSize = size,
            ContentType = GetContentType(file.FileName ?? "file"),
            IsImage = isImage,
            HasThumbnail = isImage && file.FilePath != null &&
                File.Exists(Path.Combine(_thumbnailsRoot, Path.GetFileName(file.FilePath) ?? "")),
            DateUploaded = file.DateAdded ?? file.DateInserted ?? DateTime.MinValue,
            UploadedBy = file.UserID
        };
    }
}
