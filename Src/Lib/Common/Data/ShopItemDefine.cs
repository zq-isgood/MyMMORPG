﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using SkillBridge.Message;

namespace Common.Data
{
    public class ShopItemDefine
    {
        public int ItemID { get; set; }
        public int Count { get; set; }

        public int Price { get; set; }
        public int Status { get; set; }
    }
}
