using Amazon.DynamoDBv2.DocumentModel;

namespace Backend.Models.S3
{
    public class CarCollectionList
    {
        public List<CarCollectionProperties> carCollectionList = new List<CarCollectionProperties>();

        public CarCollectionList()
        {
            carCollectionList = new List<CarCollectionProperties>();
        }

        public CarCollectionList(CarCollectionProperties carItem)
        {
            carCollectionList.Add(carItem);
        }

        public CarCollectionList(List<CarCollectionProperties> carItem)
        {
            carCollectionList = carItem;
        }

        public static List<Document> ListToDocument(CarCollectionList carCollectionList)
        {
            List<Document> listOfDocuments = new List<Document>();
            foreach(var item in carCollectionList.carCollectionList)
            {
                var carCollectionPropertiesDocument = new Document
                {
                    ["CarId"] = item.CarId,
                    ["SkinId"] = item.SkinId,
                };

                listOfDocuments.Add(carCollectionPropertiesDocument);
            }

            return listOfDocuments;
        }

        public static CarCollectionList FromDocumentToList(List<Document> documentList)
        {
            CarCollectionList carCollectionList = new CarCollectionList();
            foreach(var item in documentList)
            {
                var carId = (string)item["CarId"];
                var skinId = (string)item["SkinId"];

                var carCollection = new CarCollectionProperties(carId, skinId);
                carCollectionList.carCollectionList.Add(carCollection);
            }

            return carCollectionList;
        }
    }
}