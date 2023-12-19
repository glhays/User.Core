// -----------------------------------------------------------
// Copyright(c) Coalition of the Good-Hearted Engineers
// ======= FREE TO USE FOR THE WORLD =======
// -----------------------------------------------------------

using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace User.Core.Administration.Authorizations
{
    public class FlexibleAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        private readonly AuthorizationOptions authorizationOptions

        public FlexibleAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
            : base(options)
        {
            this.authorizationOptions = options.Value;
        }

        public override async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            var policy = await base.GetPolicyAsync(policyName);

            if(policy == null && PolicyNameHelper.IsValidPolicyName(policyName))
            {
                var permissions = PolicyNameHelper.GetPermissionFrom(policyName);

                policy = new AuthorizationPolicyBuilder()
                    .AddRequirements(new PermissionAuthorizationRequirement(permissions))
                    .Build();

                this.authorizationOptions.AddPolicy(policyName!, policy);
            }

            return policy;
        }
    }
}