using System.Collections.Generic;
using SharedPCL.Enums;

namespace SharedPCL.DataContracts
{
    public class LoginOrRegisterResponseDto
    {
        public int UserId { get; set; }
        public bool IsAuthorized { get; set; } // If username or password exists but they are wrong
        public bool IsNewlyRegisteredUser { get; set; }
        public List<CategoryTypes> SubscribedCategories { get; set; }
        public List<QuestionDto> Questions { get; set; } // Optional
    }
}
