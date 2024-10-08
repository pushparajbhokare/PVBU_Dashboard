namespace dotnetWebService.Authentication
{
    public class AuthUser
    {
        public string? Title { get; set; }
        public string? Mail { get; set; }
        public string? GivenName { get; set; }
        public string? Sn { get; set; }
        public string? Cn { get; set; }
        public string? DisplayName { get; set; }
        public string? UserId { get; set; }

        public AuthUser GetDisplayNameGivenNameMail()
        {
            return new AuthUser
            {
                DisplayName = this.DisplayName,
                GivenName = this.GivenName,
                Mail = this.Mail
            };
        }
    }
}
