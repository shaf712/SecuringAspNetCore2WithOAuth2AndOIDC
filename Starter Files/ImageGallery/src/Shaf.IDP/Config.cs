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
						new Claim("email", "frank@gmail.com"),
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
						new Claim("address", "Big Street 2"),
						new Claim("email", "claire@claire.com"),
						new Claim("role", "PayingUser"),
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
				new IdentityResources.Address(),
				new IdentityResources.Email(),
				//create a CUSTOM scope for claims that aren't caught by the common Identity Resources above
				new IdentityResource
				(
					"roles", //SCOPE name
					"YOUR role(s)",
					new List<string>() { "role" }) //name of the claim that the scope will reference 
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
                    //AllowedGrantTypes = what kind of flow are you using? For our case, we choose Hybrid flow
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
					//you need this post logout URI to successfully log out of the client application without getting a prompt to log out from the IDP also s
                    PostLogoutRedirectUris = new List<string>()
                    {
                        "https://localhost:44336/signout-callback-oidc"
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
						IdentityServerConstants.StandardScopes.Address, 
						IdentityServerConstants.StandardScopes.Email,
                        "roles"
                        //"country",
                        //"subscriptionlevel"
                    },
                    //configure client secrets to allow the client application to call the token endpoint
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    }
                }
             };
        }
    }
}
