using System.ComponentModel.DataAnnotations;

namespace EduConnect.Shared.DTOs
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Tên đăng nhập là bắt buộc")]
        [MinLength(4, ErrorMessage = "Tên đăng nhập tối thiểu 4 ký tự")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Tên đăng nhập chỉ được chứa chữ, số và dấu gạch dưới")]
        public string Username { get; set; } = default!;

        [Required(ErrorMessage = "Họ và tên là bắt buộc")]
        [StringLength(100)]
        public string FullName { get; set; } = default!;

        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = default!;

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [MinLength(6, ErrorMessage = "Mật khẩu tối thiểu 6 ký tự")]
        public string Password { get; set; } = default!;
    }
    public class GoogleLoginRequest
    {
        public string IdToken { get; set; } = string.Empty;
    }


    public class LoginRequest
    {
        [Required(ErrorMessage = "Email hoặc Tên đăng nhập là bắt buộc")]
        public string EmailOrUsername { get; set; } = default!;

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        public string Password { get; set; } = default!;
    }

    public class SendVerifyEmailRequest
    {
        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = string.Empty;
    }

    public class ConfirmEmailRequest
    {
        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mã xác thực là bắt buộc")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Mã xác thực phải gồm 6 chữ số")]
        public string Code { get; set; } = string.Empty;
    }

    public class ForgotPasswordRequest
    {
        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = string.Empty;
    }

    public class ResetPasswordRequest
    {
        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = string.Empty;

        // Token URL-safe Base64 string
        [Required(ErrorMessage = "Token là bắt buộc")]
        [StringLength(200, ErrorMessage = "Token không hợp lệ")]
        public string Token { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu mới là bắt buộc")]
        [MinLength(6, ErrorMessage = "Mật khẩu tối thiểu 6 ký tự")]
        public string NewPassword { get; set; } = string.Empty;
    }

    public class UpdateProfileRequest
    {
        [StringLength(100, ErrorMessage = "Họ tên không được vượt quá 100 ký tự")]
        public string? FullName { get; set; }

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string? Phone { get; set; }
    }

    public class ChangePasswordRequest
    {
        [Required(ErrorMessage = "Mật khẩu cũ là bắt buộc")]
        public string OldPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu mới là bắt buộc")]
        [MinLength(6, ErrorMessage = "Mật khẩu mới tối thiểu 6 ký tự")]
        public string NewPassword { get; set; } = string.Empty;
    }
}
