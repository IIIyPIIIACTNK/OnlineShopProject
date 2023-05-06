using System;
using System.Collections.Generic;

namespace OnlineShopProject.Models;

public partial class Client
{
    public int Id { get; set; }

    public string LastName { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string MiddleName { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string EMail { get; set; } = null!;
}
