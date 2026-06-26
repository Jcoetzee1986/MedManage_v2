import { Injectable } from '@angular/core';

/**
 * Tracks navigation context so "Back" buttons return to the correct origin.
 * E.g., when editing a member from a case detail page, "Back" should return to the case, not /members.
 */
@Injectable({
  providedIn: 'root'
})
export class NavigationContextService {
  private readonly STORAGE_KEY = 'medmanage_nav_context';

  /**
   * Set the return URL before navigating away.
   * Call this when opening a detail page from another context.
   */
  setReturnUrl(url: string): void {
    sessionStorage.setItem(this.STORAGE_KEY, url);
  }

  /**
   * Get the return URL (and optionally clear it).
   * Returns null if no context was set — caller should use a default.
   */
  getReturnUrl(clear = true): string | null {
    const url = sessionStorage.getItem(this.STORAGE_KEY);
    if (clear) {
      sessionStorage.removeItem(this.STORAGE_KEY);
    }
    return url;
  }

  /**
   * Clear any stored context without navigating.
   */
  clear(): void {
    sessionStorage.removeItem(this.STORAGE_KEY);
  }
}
