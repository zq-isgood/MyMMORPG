using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Network
{
    public interface IPostResponse
    {
        void PostResponse(NetMessageResponse message);
    }
}
