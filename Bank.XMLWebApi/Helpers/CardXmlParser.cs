using Bank.Entity.DTOs;
using System.Xml;

namespace Bank.XMLWebApi.Helpers
{
    public static class CardXmlParser
    {
        public static CardRequestDto ParseCardDtoFromXml(string xml)
        {
            CardRequestDto dto = new CardRequestDto();

            using var reader = XmlReader.Create(new StringReader(xml));

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.Name)
                    {
                        case "Id":
                            dto.Id = int.Parse(reader.ReadElementContentAsString());
                            break;
                        case "CardType":
                            dto.CardType = reader.ReadElementContentAsString();
                            break;
                        case "Limit":
                            dto.Limit = decimal.Parse(reader.ReadElementContentAsString());
                            break;
                        case "Status":
                            dto.Status = reader.ReadElementContentAsString();
                            break;
                        case "ExpirationDate":
                            dto.Date = DateTime.Parse(reader.ReadElementContentAsString());
                            break;
                        case "UserId":
                            int userId = int.Parse(reader.ReadElementContentAsString());
                            dto.FullName = $"User #{userId}"; // Örnek: Gerçek isim çekilecekse ayrı servis gerekir
                            break;
                    }
                }
            }

            return dto;
        }
    }


}
