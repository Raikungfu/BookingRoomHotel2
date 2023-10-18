using BookingRoomHotel.Models;
using System.ComponentModel.DataAnnotations;

namespace BookingRoomHotel.ViewModels
{
    public class CustomerViewModel
    {
        public CusRegisterViewModel CusRegisterViewModel { get; set; }
        public CusLoginViewModel CusLoginViewModel { get; set; }
        public CusChangePwViewModel CusChangePwViewModel { get; set; }
        public CusForgotPasswordViewModel CusForgotPasswordViewModel { get; set; }
        public CusViewModel CusViewModel { get; set; }

    }
    public class CusRegisterViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        [Compare("PwCf")]
        public string Pw { get; set; }
        [Compare("Pw")]
        public string PwCf { get; set; }
    }

    public class CusLoginViewModel
    {

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your username")]
        [StringLength(maximumLength: 20, MinimumLength = 5, ErrorMessage = "Length between 5 - 20")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [StringLength(maximumLength: 30, MinimumLength = 6, ErrorMessage = "Length between 6 - 30")]
        public string Password { get; set; }
    }

    public class CusChangePwViewModel
    {
        public string Id { get; set; }
        public string OldPw { get; set; }
        [Compare("ConfirmNewPw")]
        public string NewPw { get; set; }
        [Compare("NewPw")]
        public string ConfirmNewPw { get; set; }
    }

    public class CusViewModel
    {
        public int Name { get; set; }
    }

    public class CusForgotPasswordViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
    }
    public class ListCustomerViewModel
    {
        public List<Customer> ListCus { get; set; }
        public int Count { get; set; }
    }
}
