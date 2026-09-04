using System.Net.Http;

namespace NetworkWebRecon.Services
{
    public class XssService
    {
        public async Task<bool> TestReflectionAsync(
            string url,
            string parameter)
        {
            try
            {
                using HttpClient client = new HttpClient();

                client.Timeout =
                    TimeSpan.FromSeconds(5);

                // قيمة اختبار عشوائية
                string testValue =
                    "XSS_TEST_12345";

                string separator =
                    url.Contains("?") ? "&" : "?";

                string testUrl =
                    $"{url}{separator}{parameter}={testValue}";

                // إرسال الطلب
                string response =
                    await client.GetStringAsync(testUrl);

                // هل القيمة ظهرت في الـ Response؟
                return response.Contains(testValue);
            }
            catch
            {
                return false;
            }
        }
    }
}