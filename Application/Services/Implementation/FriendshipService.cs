using Application.DTOs.FriendshipDtos;
using Application.Interfaces.Repositories;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Models;

namespace Application.Services.Implementation
{
    public class FriendshipService : IFriendshipService
    {
        private readonly IFriendshipRepository _friendshipRepository;
        private readonly IMapper _mapper;
        public FriendshipService(IFriendshipRepository friendshipRepository, IMapper mapper)
        {
            _friendshipRepository = friendshipRepository;
            _mapper = mapper;
        }
        public async Task<FriendshipDto> CreateFriendshipAsync(FriendshipCreateDto friendshipData)
        {
            if (friendshipData.UserId == friendshipData.FriendId)
                throw new InvalidOperationException("You can't be friends with yourself.");

            var exists = await _friendshipRepository.ExistsAsync(friendshipData.UserId, friendshipData.FriendId);
            if (exists)
                throw new InvalidOperationException("Friendship already exists.");

            var friendship = new Friendship
            {
                UserId = friendshipData.UserId,
                FriendId = friendshipData.FriendId
            };

            var created = await _friendshipRepository.CreateAsync(friendship);
            return _mapper.Map<FriendshipDto>(created);
        }

        public async Task<IEnumerable<FriendshipDto>> GetFriendsForUserAsync(int userId)
        {
            var friendships = await _friendshipRepository.GetUserFriendshipsAsync(userId);
            return _mapper.Map<IEnumerable<FriendshipDto>>(friendships);
        }

        public async Task<bool> DeleteFriendshipAsync(int userId, int friendId)
        {
            return await _friendshipRepository.DeleteAsync(userId, friendId);
        }
    }
}
