using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvImport
{
    class Program
    {
        static void Main(string[] args)
        {
            // csv 読み込み
            // \mark 有る場合、@を付ける
            const string CSV_PATH = @"C:\Users\owner\Desktop\C#特訓\オブジェクト指向\住所録\personal_infomation.csv";
            var list = ReadCsv(CSV_PATH);
            RegistAddresses(list);

        }

        static List<Address> ReadCsv(string path)
        {
            List<Address> list = new List<Address>();

            var encode = new System.Text.UTF8Encoding(false);

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ",",
                NewLine = Environment.NewLine,
                IgnoreBlankLines = true,
                Encoding = Encoding.UTF8,
                AllowComments = true,
                Comment = '#',
                DetectColumnCountChanges = true,
                TrimOptions = TrimOptions.Trim,
            };


            using (var reader = new StreamReader(path, Encoding.UTF8))
            //using (var reader = new System.IO.StreamReader(path, encode))
            {
                var csv = new CsvReader(reader, config);
                //var csv = new CsvHelper.CsvReader(reader,encode);

                    bool isSkip = true;

                while (csv.Read())
                {
                    if (isSkip)
                    {
                        isSkip = false;
                        continue;
                    }

                    //氏名,氏名（カタカナ）,電話番号,メールアドレス,郵便番号,住所,,,,
                    // 0    1                 3         4             5        6 7 8 9
                    Address address = new Address()
                    {
                        Name = csv.GetField<string>(0),
                        Kana = csv.GetField<string>(1),
                        Telephone = csv.GetField<string>(2),
                        Mail = csv.GetField<string>(3),
                        ZipCode = csv.GetField<string>(4).Replace("-", ""),
                        Prefecture = csv.GetField<string>(5),
                        StreetAddress = $"{csv.GetField<string>(6)}{csv.GetField<string>(7)}{csv.GetField<string>(8)}{csv.GetField<string>(9)}"
                    };

                    list.Add(address);
                }
            }

            return list;
        }

        static void RegistAddresses(List<Address> addresses)
        {
            using (var db = new AddressBookInfoEntities())
            {
                // 内容設定
                db.Addresses.AddRange(addresses);
                // 登録
                db.SaveChanges();
            }
        }


    }
}
