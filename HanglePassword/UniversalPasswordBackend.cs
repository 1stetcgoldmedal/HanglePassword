using System;
using System.Text;
using System.Security.Cryptography;

namespace SecurityControls
{
    public static class UniversalPasswordBackend
    {
        /// <summary>
        /// 다국어(유니코드) 평문 비밀번호를 안전하게 해싱하여 반환합니다.
        /// (WinForms, WPF, Console 공통 사용 가능)
        /// </summary>
        /// <param name="plainUnicodePassword">UI 단에서 받아온 다국어 평문</param>
        /// <param name="salt">보안 강화를 위한 고유 솔트값 (선택사항)</param>
        /// <returns>SHA-256 변환된 16진수 해시 문자열</returns>
        public static string ProcessToSecureHash(string plainUnicodePassword, string salt = "MyCustomSaltKey123!")
        {
            if (string.IsNullOrEmpty(plainUnicodePassword))
                return string.Empty;

            // 평문 + 솔트 결합
            string saltedPassword = plainUnicodePassword + salt;

            // 유니코드(UTF-8) 바이트로 변환하여 다국어 데이터 손실 방지
            byte[] bytes = Encoding.UTF8.GetBytes(saltedPassword);

            // SHA-256 해싱
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(bytes);

                // 결과를 소문자 16진수(Hex) 문자열로 변환
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }

                // 사용이 끝난 원본 바이트 배열 메모리 덮어쓰기 (Zeroing)
                Array.Clear(bytes, 0, bytes.Length);

                return builder.ToString();
            }
        }
    }
}