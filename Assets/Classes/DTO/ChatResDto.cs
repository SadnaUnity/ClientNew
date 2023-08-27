using System;
using System.Collections.Generic;

namespace Classes.DTO
{
    [Serializable] 
    public class ChatResDto
    {
        public string message { set; get; }
        public List<MsgDto> messageList { set; get; }

        public ChatResDto()
        {
            
        }
    }
}