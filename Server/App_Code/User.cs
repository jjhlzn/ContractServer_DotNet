﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// User 的摘要说明
/// </summary>
public class User
{
	public User()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}

    public String id { get; set; }
    public String department { get; set; }
    public String name { get; set; }
    public String password { get; set; }
    public String deviceToken { get; set; }
    public String platform { get; set; }
    public int badge { get; set; }
}