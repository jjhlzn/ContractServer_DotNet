using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Dapper;
using log4net;
using log4net.Repository.Hierarchy;

/// <summary>
/// ProductService 的摘要说明
/// </summary>
public class ProductService
{
    private ILog Logger = LogManager.GetLogger(typeof (Product));

    public GetProductResponse GetProductById(String productId)
    {
        GetProductResponse resp = new GetProductResponse();
        string sql = @"select spzwmc as chineseName,
                        spywmc as englishName,
                        sphh as huohao,
                        spggms as chineseDesc,
                        spggywms as englishDesc, 
                        cbdj as chengbenPrice, 
                        wxdj as sellPrice,
                        spgg as xinghao,
                        yw_spbm as id,
                        txm as barCode

                        from yw_commodity
                        where txm= @productId";
        Logger.Debug(sql);
        using (var conn = ConnectionFactory.GetInstance())
        {
            var product = conn.Query<Product>(sql, new { productId }).FirstOrDefault();
            if (product == null)
            {
                product = new Product();
                resp.isExist = false;
            }
            else
            {
                resp.isExist = true;
                Random random = new Random();


                byte[] image = GetImageById(GetRandomProductId());
                if (image == null)
                {
                    product.hasImage = false;
                }
                else
                {
                    product.hasImage = true;
                    product.image = image;
                }
            }
            
            resp.product = product;
        }
        return resp;
    }

    private String GetRandomProductId()
    {
        Random random = new Random(DateTime.Now.Millisecond);
        int value = random.Next(2);
        if (value == 0)
        {
            return "";
        }
        else
        {
            String[] ids = new string[] {"2502002543", "2502001089", "2502001085", "I230014", "C055008", "Q030208", "Q030206", "1003000407"};
            return ids[random.Next(ids.Length)];
        }
    }

    public byte[] GetImageById(String id)
    {

        GetProductResponse resp = new GetProductResponse();
        string sql = @"select picture_file as image
                        from yw_commodity_picture
                        where yw_spbm= @id and picture_lx='主图' and picture_xz='主图'";
        Logger.Debug(sql);
        using (var conn = ConnectionFactory.GetImageInstance())
        {
            var product = conn.Query<Product>(sql, new { id }).FirstOrDefault();
            if (product == null)
            {
                return null;
            }
            else
            {
                return product.image;
            }

        }
    }

    

    public GetProductResponse GetImageById3(String id)
    {

        GetProductResponse resp = new GetProductResponse();
        string sql = @"select picture_file as image
                        from yw_commodity_picture
                        where yw_spbm= @id and picture_lx='主图' and picture_xz='主图'";
        Logger.Debug(sql);
        using (var conn = ConnectionFactory.GetImageInstance())
        {
            var product = conn.Query<Product>(sql, new { id }).FirstOrDefault();
            if (product == null)
            {
                product = new Product();
                resp.isExist = false;
            }
            else
            {
                resp.isExist = true;
            }

            resp.product = product;
        }
        return resp;
    }

    public GetProductResponse GetImageById2(String productId)
    {

        var imgPath = @"c:\docker.png";
        //从图片中读取byte
        var imgByte = File.ReadAllBytes(imgPath);
        Logger.Debug("imgByte.size = " + imgByte.Count());
        GetProductResponse resp = new GetProductResponse();
        Product product = new Product();
        product.image = imgByte;
        resp.product = product;
        return resp;
    }

   
}