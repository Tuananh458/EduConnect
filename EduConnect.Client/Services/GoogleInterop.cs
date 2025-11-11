using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace EduConnect.Client.Services
{
    public static class GoogleInterop
    {
        [JSInvokable("HandleGoogleToken")]
        public static async Task HandleGoogleToken(string idToken)
        {
            try
            {
                Console.WriteLine($"[GoogleInterop] IdToken nhận được: {idToken[..15]}...");

                using var scope = Program.ServiceProvider.CreateScope();
                var auth = scope.ServiceProvider.GetRequiredService<AuthService>();
                var nav = scope.ServiceProvider.GetRequiredService<NavigationManager>();

                var (success, message) = await auth.GoogleLoginAsync(idToken);

                if (success)
                {
                    Console.WriteLine($"[GoogleInterop] ✅ Đăng nhập Google thành công: {message}");
                    await Task.Delay(500);
                    nav.NavigateTo("/", true);
                }
                else
                {
                    Console.WriteLine($"[GoogleInterop] ❌ Đăng nhập Google thất bại: {message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[GoogleInterop] ❌ Lỗi xử lý IdToken: {ex.Message}");
            }
        }
    }
}
