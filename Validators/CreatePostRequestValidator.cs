using CheerMeApp.Contracts.V1.Requests;
using FluentValidation;

namespace CheerMeApp.Validators
{
    public class CreatePostRequestValidator : AbstractValidator<CreatePostRequest>
    {
        public CreatePostRequestValidator()
        {
            RuleFor(request => request.PostText).NotEmpty();
        }
    }
}