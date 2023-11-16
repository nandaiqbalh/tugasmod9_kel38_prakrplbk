using ApiKel38.Models.Dto;

namespace ApiKel38.Data
{
    public static class UserStore
    {
        public static List<UserDTO> userList = new List<UserDTO>
        {
             new UserDTO{Id=1, Username="nandaiqbalh", Password="12345678"},
             new UserDTO{Id=2, Username="nanndaa", Password="12345678"}
        };

    }
}
