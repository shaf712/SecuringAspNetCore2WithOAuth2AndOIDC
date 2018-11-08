using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Shaf.IDP
{
    public static class Config
    {
        //holds IN-MEMORY configuration 

        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    //the subjectId is  their unique identifier 
                    SubjectId = "d860efca-22d9-47fd-8249-791ba61b07c7",
                    Username = "Frank",
                    Password = "password",

                    Claims = new List<Claim>
                    {
                        //a "claim" represents information about a user 
                        new Claim("given_name", "Frank"),
                        new Claim("family_name", "Underwood"),
                        new Claim("address", "Main Road 1"),
                        new Claim("role", "FreeUser"),
                        new Claim("subscriptionlevel", "FreeUser"),
                        new Claim("country", "nl")
                    }
                },
                new TestUser
                {
                    SubjectId = "b7539694-97e7-4dfe-84da-b4256e1ff5c7",
                    Username = "Claire",
                    Password = "password",

                    Claims = new List<Claim>
                    {
                        new Claim("given_name", "Claire"),
                        new Claim("family_name", "Underwood"),
                        //new Claim("address", "Big Street 2"),
                        //new Claim("role", "PayingUser"),
                        //new Claim("subscriptionlevel", "PayingUser"),
                        //new Claim("country", "be")
                    }
                }
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>()
            {
                //THESE ARE SCOPES

                //Identity Resources map to scopes that give identity-related information 
                //since we're using Open ID connect, we must support the Open ID scope 
                new IdentityResources.OpenId(),
                //OpenId() ensures that the SubjectId is included 
                //if the client requests the OpenId() scope, the the user identiifer claim (which is SubjectId) is  returned
                new IdentityResources.Profile(),
                //the Profile scope maps to profile-related claims like the ones in the list of claims
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>()
            {
                new Client
                {
                    ClientName = "Image Gallery",
                    ClientId = "imagegalleryclient",
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    AccessTokenType = AccessTokenType.Reference,
                    //IdentityTokenLifetime = ...
                    //AuthorizationCodeLifetime = ...
                    AccessTokenLifetime = 120,
                    AllowOfflineAccess = true,
                    //AbsoluteRefreshTokenLifetime = ...
                    UpdateAccessTokenClaimsOnRefresh = true,
                    RedirectUris = new List<string>()
                    {
                        "https://localhost:44336/signin-oidc"
                    },
                    PostLogoutRedirectUris = new List<string>()
                    {
                        "https://localhost:44336/signout-callback-oidc"
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                        "roles",
                        "imagegalleryapi",
                        "country",
                        "subscriptionlevel"
                    },
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    }
                }
             };
        }
    }
}
