using System;
using ExtensionMinder;
using GoogleApi.Entities.Common.Enums;
using Vita.Contracts;

namespace Vita.Domain.Infrastructure
{
  public static class CategoryTypeConverter
  {
    public static CategoryType Convert(string text)
    {
      if (string.IsNullOrWhiteSpace(text)) throw new ArgumentException();

      if (TypeMatch(text, out var valueOrDefault)) return valueOrDefault;
      return CategoryType.Uncategorised;
    }

    public static string ExtractSubCategory(string text)
    {
      if (string.IsNullOrEmpty(text)) return null;

      if (!text.Contains("-")) return text;

      return  text.Split(System.Convert.ToChar("-"))[1].Clean().Replace(" ", string.Empty).Trim();

    }

    public static CategoryType Convert(PlaceLocationType place)
    {
      switch (place)
      {
        case PlaceLocationType.Accounting:
        case PlaceLocationType.Atm:
        case PlaceLocationType.Bank:
          return CategoryType.BankingFinance;

        case PlaceLocationType.Airport:
        case PlaceLocationType.Travel_Agency:
        case PlaceLocationType.Lodging:
          return CategoryType.HolidayTravel;

        case PlaceLocationType.Amusement_Park:
        case PlaceLocationType.Art_Gallery:
        case PlaceLocationType.Bowling_Alley:
        case PlaceLocationType.Zoo:
        case PlaceLocationType.Campground:
        case PlaceLocationType.Florist:
        case PlaceLocationType.Movie_Rental:
        case PlaceLocationType.Movie_Theater:
        case PlaceLocationType.Moving_Company:
        case PlaceLocationType.Natural_Feature:
        case PlaceLocationType.Night_Club:
        case PlaceLocationType.Rv_Park:
        case PlaceLocationType.Casino:
          return CategoryType.Entertainment;

        case PlaceLocationType.Bakery:
        case PlaceLocationType.Convenience_Store:
        case PlaceLocationType.Grocery_Or_Supermarket:
        case PlaceLocationType.Supermarket:
        case PlaceLocationType.Liquor_Store:
          return CategoryType.Groceries;

        case PlaceLocationType.Cafe:
        case PlaceLocationType.Bar:
        case PlaceLocationType.Restaurant:
        case PlaceLocationType.Meal_Delivery:
        case PlaceLocationType.Meal_Takeaway:
        
          return CategoryType.FoodDrinks;

        case PlaceLocationType.Car_Dealer:
        case PlaceLocationType.Car_Rental:
        case PlaceLocationType.Car_Repair:
        case PlaceLocationType.Car_Wash:
        case PlaceLocationType.Taxi_Stand:
        case PlaceLocationType.Bus_Station:
        case PlaceLocationType.Train_Station:
        case PlaceLocationType.Transit_Station:
        case PlaceLocationType.Subway_Station:
        case PlaceLocationType.Gas_Station:
        case PlaceLocationType.Park:
        case PlaceLocationType.Parking:
          return CategoryType.Transport;

        case PlaceLocationType.Dentist:
        case PlaceLocationType.Doctor:
        case PlaceLocationType.Health:
        case PlaceLocationType.Beauty_Salon:
        case PlaceLocationType.Hair_Care:
        case PlaceLocationType.Pharmacy:
        case PlaceLocationType.Physiotherapist:
        case PlaceLocationType.Spa:
        case PlaceLocationType.Gym:
          return CategoryType.HealthBeauty;

        case PlaceLocationType.Clothing_Store:
        case PlaceLocationType.Shopping_Mall:
        case PlaceLocationType.Shoe_Store:
        case PlaceLocationType.Bicycle_Store:
        case PlaceLocationType.Book_Store:
        case PlaceLocationType.Department_Store:
        case PlaceLocationType.Electronics_Store:
        case PlaceLocationType.Furniture_Store:
        case PlaceLocationType.Hardware_Store:
        case PlaceLocationType.Jewelry_Store:
        case PlaceLocationType.Pet_Store:
        case PlaceLocationType.Home_Goods_Store:
        case PlaceLocationType.Store:        
          return CategoryType.Shopping;

        case PlaceLocationType.Electrician:
        case PlaceLocationType.Plumber:
        case PlaceLocationType.Roofing_Contractor:
        case PlaceLocationType.Floor:
        case PlaceLocationType.Locksmith:
        case PlaceLocationType.Painter:
        case PlaceLocationType.Real_Estate_Agency:
        case PlaceLocationType.Street_Address:
        case PlaceLocationType.General_Contractor:
          return CategoryType.Home;
        case PlaceLocationType.School:
          return CategoryType.Kids;

        case PlaceLocationType.University:
          return CategoryType.WorkStudy;

        case PlaceLocationType.Insurance_Agency:
          return CategoryType.Insurance;

        default:
          return CategoryType.Uncategorised;
      }
    }

    private static bool TypeMatch(string text, out CategoryType category)
    {
      if (text.Contains("-"))
      {
        string first = text.Split(System.Convert.ToChar("-"))[0].Trim();
        try
        {
          var done = first.ParseAsEnumByDescriptionAttribute<CategoryType>();
          {
            category = done;
            return true;
          }
        }
        catch (Exception e)
        {
          Console.WriteLine(e.Message);
        }

        try
        {
          var done = first.TryParseEnum<CategoryType>();
          if (done.HasValue)
          {
            category = done.GetValueOrDefault();
            return true;
          }
        }
        catch (Exception e)
        {
          Console.WriteLine(e.Message);
        }
      }


      try
      {
        var done = text.ParseAsEnumByDescriptionAttribute<CategoryType>();
        {
          category = done;
          return true;
        }
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
      }

      try
      {
        var done = text.TryParseEnum<CategoryType>();
        if (done.HasValue)
        {
          category = done.GetValueOrDefault();
          return true;
        }
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
      }

      category = CategoryType.Uncategorised;
      return false;
    }
  }
}