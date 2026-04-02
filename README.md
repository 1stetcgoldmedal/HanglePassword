# 🛡️ UnicodeSecurePassword

Why are we still forcing users to mix uppercase, lowercase, and weird symbols (`!@#$`) just to make a password "secure"?

This project started from a simple question: **Why can't we just use our native languages for passwords?** Most password inputs (`type="password"`) block IME (Input Method Editor) due to legacy encoding fears and keylogger prevention. This forces non-English speakers to memorize meaningless ASCII strings. `UnicodeSecurePassword` is a UI/UX library (Web & WPF) that perfectly supports multi-language IME inputs while keeping the password field visually masked and memory-safe.

---

### 🧮 The Math: Why Unicode? (Information Entropy)
It's not just about UX; it's about security density. The strength of a password (Entropy) is calculated as:

$E = L \times \log_2(R)$ *(where $L$ is length, and $R$ is the size of the character set)*

* **English (Lowercase, 8 chars):** $R=26 \implies 8 \times \log_2(26) \approx 37.6 \text{ bits}$
* **Korean Hangul (8 chars):** $R=11172 \implies 8 \times \log_2(11172) \approx 107.6 \text{ bits}$

A short, easy-to-remember 6-character Hangul password provides exponentially higher brute-force resistance than a 12-character complex English password. You get better UX and vastly better security.

---

### ✨ Features
* **Native IME Support:** Type in Korean, Chinese, Japanese, Russian, etc., without the browser or OS blocking it.
* **Pixel-Perfect Masking (WPF):** Handling variable-width characters with masking (`＊`) usually breaks the caret and selection sync. We fixed this by manually rendering a fake caret and selection block. It looks and feels exactly like a native password box.
* **Anti-Scraping / Memory Safe:** * **Web:** Captures the value, immediately hashes it on the client-side, and wipes the DOM.
  * **WPF:** Blocks UI Automation (Spy++, Inspect.exe) and disables clipboard hijacking.

---

### 💻 How to Use
* **Web (HTML/JS):** Check out `Web/index.html`. Uses `-webkit-text-security` and a simple DOM-clearing script.
* **Desktop (WPF):** Drop `UniversalSecurePasswordBox.xaml` into your project. 
* **WinForms / Console:** Don't reinvent the wheel. Host the WPF control via `ElementHost` for WinForms, or launch it as a secure popup for Console apps.

---
**License:** MIT