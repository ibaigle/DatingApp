using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOS;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class MessagesController : BaseApiController
    {
        /* private readonly IUserRepository _userRepository;
        private readonly IMessageRepository _messageRepository; */
        public readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public MessagesController(IUnitOfWork unitOfWork/* IUserRepository userRepository, IMessageRepository messageRepository */, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            /* _messageRepository = messageRepository;
            _userRepository = userRepository; */
        }

        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
        {
            var username = User.GetUsername();
            if (username == createMessageDto.RecipientUsername.ToLower())
                return BadRequest("You cannot send messages to yourself");
            var sender = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
            var recipient = await _unitOfWork.UserRepository.GetUserByUsernameAsync(createMessageDto.RecipientUsername);

            if (recipient == null) return NotFound();

            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUsername = sender.UserName,
                RecipientUsername = recipient.UserName,
                Content = createMessageDto.Content
            };

            _unitOfWork.MessageRepository.AddMessage(message);
            if (await _unitOfWork.Complete()) return Ok(_mapper.Map<MessageDto>(message));
            return BadRequest("Failed to send Message");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessagesForUser([FromQuery] MessageParams messageParams){
            messageParams.Username= User.GetUsername();

            var messages = await _unitOfWork.MessageRepository.GetMessagesForUser(messageParams);

            Response.AddPaginationHeader(messages.CurrentPage, 
                messages.PageSize, messages.TotalCount, messages.TotalPages);
            return messages;
        }
        /* [HttpGet("thread/{username}")]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageThread (string username){
            var currentUsername= User.GetUsername();

            return Ok(await _messageRepository.GetMessageThread(currentUsername,username));
        } */

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id){
            var username = User.GetUsername();
            var message= await _unitOfWork.MessageRepository.GetMessage(id);
            if(message.Sender.UserName != username && message.Recipient .UserName != username)
                return Unauthorized();
            if(message.Sender.UserName == username )message.SenderDeleted=true;
            if(message.Recipient.UserName == username)message.RecipientDeleted = true;
            if(message.SenderDeleted && message.RecipientDeleted)
                _unitOfWork.MessageRepository.DeleteMessage(message);
            if(await _unitOfWork.Complete()) return Ok();

            return BadRequest("Problem deleting the message");
        }

    }
}