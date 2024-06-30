using System.ComponentModel.DataAnnotations;
using System.Reflection;

using DB.Domain.Entities;

namespace DB.SD
{

    public enum Roles
    {
        Admin = 1,
        User = 2
    }

    public class DropDownModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public static class ImageHelper
    {
        public static string GetImageDataUrl(byte[] imageData, string extension)
        {
            string base64Image = Convert.ToBase64String(imageData);
            return $"data:image/{extension};base64,{base64Image}";
        }
    }

    public static class EnumHelper
    {

        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()
                            ?.GetName() ?? enumValue.ToString();
        }

        public static List<DropDownModel> GetEnumDisplayNames<T>() where T : Enum
        {
            var type = typeof(T);

            return Enum.GetValues(type)
                       .Cast<T>()
                       .Select(x =>
                       {
                           var memberInfo = type.GetMember(x.ToString()).First();
                           var displayAttribute = memberInfo.GetCustomAttribute<DisplayAttribute>();
                           var displayName = displayAttribute?.Name ?? x.ToString();

                           return new DropDownModel
                           {
                               Id = Convert.ToInt32(x),
                               Name = displayName
                           };
                       })
                       .ToList();
        }
    }

    public static class AuctionSD
    {
        public static bool IsInstantBuy(DB.Domain.Entities.AuctionType type)
        {

            return type == AuctionType.Instant ||
                type == AuctionType.Days_7_With_Instant ||
                type == AuctionType.Days_14_With_Instant;
        }

        public static bool IsAuction(DB.Domain.Entities.AuctionType type)
        {
            return
                type == AuctionType.Days_7_With_Instant ||
                type == AuctionType.Days_14_With_Instant ||
                type == AuctionType.Days_14 ||
                type == AuctionType.Days_7;

        }
    }

    public static class SD
    {

        public static string DateTimeFullFormat = "yyyy-MM-dd:HH:mm";
        public static string SavePath = "C:\\Source";
    }
}
