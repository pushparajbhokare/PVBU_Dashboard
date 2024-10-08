using App.Configurations;
using dotnetWebService.Core.DB;
using System;
using System.DirectoryServices;

namespace dotnetWebService.Authentication
{
    public class LdapAuthentication : IAuthentication
    {
        private readonly LdapConfig config;
        private readonly UserService userService;

        public LdapAuthentication(LdapConfiguration LdapConfiguration, UserService UserService)
        {
            this.config = LdapConfiguration.config;
            userService = UserService;
        }

        public AuthUser Login(string userId, string password)
        {

            if (!config.EnableAuth)
            {
                return SetUpUser(userId, "Anonymous", "Anonymous", "Anonymous", "Anonymous", "Anonymous", "Anonymous");
            }

            try
            {
                if ("true".Equals(config.UseServerCredentials, StringComparison.OrdinalIgnoreCase))
                {
                    using (DirectoryEntry entry = new(config.Path, config.UserName, config.Password, "NONE" == config.AuthenticationType ? AuthenticationTypes.None : AuthenticationTypes.Secure))
                    {
                        //Console.WriteLine("Created directory entry..............");
                        return AuthenticateUser(userId, password, entry);
                    }
                }
                else
                {
                    using (DirectoryEntry entry = new(config.Path, userId, password, "NONE" == config.AuthenticationType ? AuthenticationTypes.None : AuthenticationTypes.Secure))
                    {
                        //Console.WriteLine("Created directory entry..............");
                        return AuthenticateUser(userId, password, entry);
                    }
                }
            }
            catch (Exception ex)
            {
                // if we get an error, it means we have a login failure.
                // Log specific exception
                Console.WriteLine($" stack trace = {ex.StackTrace}");
                Console.WriteLine(ex.Message);
            }
            // userService.DeleteUser(userId);
            return null;
        }

        public AuthUser AddUserByAdmin(string adminPassword, string adminUserId, string userId)
        {
            try
            {
                if ("true".Equals(config.UseServerCredentials, StringComparison.OrdinalIgnoreCase))
                {
                    using (DirectoryEntry entry = new(config.Path, config.UserName, config.Password, "NONE" == config.AuthenticationType ? AuthenticationTypes.None : AuthenticationTypes.Secure))
                    {
                        //Console.WriteLine("Created directory entry..............");
                        return AuthenticateUser(userId, adminPassword, entry);
                    }
                }
                else
                {
                    using (DirectoryEntry entry = new(config.Path, adminUserId, adminPassword, AuthenticationTypes.Secure))
                    {
                        //Console.WriteLine("Created directory entry..............");
                        //return AuthenticateUser(adminUserId, adminPassword, entry);
                        using (DirectorySearcher searcher = new(entry))
                        {
                            //Console.WriteLine($"created directory searcher ---------------- {searcher}");
                            //searcher.Filter = String.Format(config.Filter = userId);
                            searcher.Filter = String.Format(config.Filter + "=" + userId);

                            //Console.WriteLine($"searching with ---------------- {searcher.Filter}");

                            var result = searcher.FindOne();

                            //Console.WriteLine($"result = {result}");

                            if (result == null)
                            {
                                return null;
                            }

                            var AuthTitle = result.Properties[config.AuthTitle];
                            var AuthMail = result.Properties[config.AuthMail];
                            var AuthGivenName = result.Properties[config.AuthGivenName];
                            var AuthSn = result.Properties[config.AuthSn];
                            var AuthCn = result.Properties[config.AuthCn];
                            var AuthDisplayName = result.Properties[config.AuthDisplayName];
                            string fullName = AuthDisplayName == null || AuthDisplayName.Count <= 0 ? "" : AuthDisplayName[0].ToString();
                            string email = AuthMail == null || AuthMail.Count <= 0 ? "" : AuthMail[0].ToString();

                            dotnetWebService.Model.User newUser = new();
                            newUser.username = userId;
                            newUser.full_name = fullName;
                            newUser.email = email;
                            int addedUser = userService.AddUser(newUser);
                            AuthUser AuthUser = new AuthUser
                            {
                                GivenName = userId,
                                DisplayName = fullName,
                                Mail = email
                            };

                            return addedUser == 1 ? AuthUser.GetDisplayNameGivenNameMail() : null; 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // if we get an error, it means we have a login failure.
                // Log specific exception
                Console.WriteLine($" stack trace = {ex.StackTrace}");
                Console.WriteLine(ex.Message);
            }
            // userService.DeleteUser(userId);
            return null;
        }
        protected AuthUser AuthenticateUser(string userId, string password, DirectoryEntry entry)
        {
            using (DirectorySearcher searcher = new(entry))
            {
                //Console.WriteLine($"created directory searcher ---------------- {searcher}");
                searcher.Filter = String.Format("{0}={1}", config.Filter, userId);

                //Console.WriteLine($"searching with ---------------- {searcher.Filter}");

                var result = searcher.FindOne();

                //Console.WriteLine($"result = {result}");

                if (result == null)
                {
                    return null;
                }

                var AuthTitle = result.Properties[config.AuthTitle];
                var AuthMail = result.Properties[config.AuthMail];
                var AuthGivenName = result.Properties[config.AuthGivenName];
                var AuthSn = result.Properties[config.AuthSn];
                var AuthCn = result.Properties[config.AuthCn];
                var AuthDisplayName = result.Properties[config.AuthDisplayName];
                string fullName = AuthDisplayName == null || AuthDisplayName.Count <= 0 ? "" : AuthDisplayName[0].ToString();
                string email = AuthMail == null || AuthMail.Count <= 0 ? "" : AuthMail[0].ToString();

                dotnetWebService.Model.User newUser = new();
                newUser.username = userId;
                newUser.full_name = fullName;
                newUser.email = email;
                userService.AddUser(newUser);

                //Console.WriteLine($"{AuthTitle[0]}");
                //Console.WriteLine($"{AuthMail[0]}");
                return SetUpUser(
                    userId,
                    AuthTitle == null || AuthTitle.Count <= 0 ? "" : AuthTitle[0].ToString(),
                    email,
                    AuthGivenName == null || AuthGivenName.Count <= 0 ? "" : AuthGivenName[0].ToString(),
                    AuthSn == null || AuthSn.Count <= 0 ? "" : AuthSn[0].ToString(),
                    AuthCn == null || AuthCn.Count <= 0 ? "" : AuthCn[0].ToString(),
                    fullName
                );
            }
        }


        private AuthUser SetUpUser(string userId, string title, string mail, string givenName, string sn, string cn, string displayName)
        {
            return new AuthUser
            {
                UserId = userId,
                Title = title,
                Mail = mail,
                GivenName = givenName,
                Sn = sn,
                Cn = sn,
                DisplayName = displayName
            };
        }
    }
}
