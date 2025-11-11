// =========================
// GOOGLE LOGIN HANDLER
// =========================
window.loadGoogleSignIn = async function () {
    try {
        // Khởi tạo SDK Google
        google.accounts.id.initialize({
            //
            callback: handleGoogleCredentialResponse
        });

        // Hiển thị nút đăng nhập
        google.accounts.id.renderButton(
            document.getElementById("googleSignInContainer"),
            { theme: "outline", size: "large", width: 360 }
        );
    } catch (e) {
        console.error("[Google] loadGoogleSignIn error:", e);
    }
};

// =========================
// CALLBACK SAU KHI LOGIN
// =========================
async function handleGoogleCredentialResponse(response) {
    try {
        if (!response || !response.credential) {
            console.error("[Google] Không có credential từ Google");
            return;
        }

        const idToken = response.credential;
        console.log("[Google] IdToken nhận được:", idToken.substring(0, 25) + "...");

        // Gọi hàm C# qua JSInterop
        const result = await DotNet.invokeMethodAsync("EduConnect.Client", "HandleGoogleToken", idToken);
        console.log("[Google] Đăng nhập kết quả:", result);
    } catch (err) {
        console.error("[Google] Lỗi khi gửi token:", err);
    }
}
