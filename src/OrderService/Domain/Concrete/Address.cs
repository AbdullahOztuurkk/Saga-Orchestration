using CoreLib.Entity.Concrete;
using Microsoft.EntityFrameworkCore;

namespace Domain.Concrete;

[Owned]
public class Address
{
    public string Line { get; set; }
    public string Province { get; set; }
    public string District { get; set; }
}
