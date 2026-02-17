using System;
using System.ComponentModel;
using System.Reflection;
using IscrizioniManager.Models;

public class AnnoScolasticoItem
{
    public int? Value { get; }
    public string Description { get; }

    public AnnoScolasticoItem(int value)
    {
        Value = value;
        Description = GetEnumDescription((AnnoScolastico)value);
    }

  public AnnoScolasticoItem(int? value, string desc)
  {
    Value = value;
    Description = desc;
  }

  private static string GetEnumDescription(Enum value)
    {
        var fi = value.GetType().GetField(value.ToString());
        var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
        return attributes.Length > 0 ? attributes[0].Description : value.ToString();
    }
}
