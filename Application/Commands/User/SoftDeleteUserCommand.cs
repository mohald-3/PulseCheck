namespace Application.Commands.User
{
    using global::Application.Common;
    using MediatR;

    namespace Application.Commands.User
    {
        public class SoftDeleteUserCommand : IRequest<OperationResult<bool>>
        {
            public int UserId { get; set; }

            public SoftDeleteUserCommand(int userId)
            {
                UserId = userId;
            }
        }
    }

}
