using OpenCvSharp;
using System;

namespace METU.Win
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
          
                 //人脸特征库 github OpenCV下载下来复制 否则会有问题
                 //实例检测的级联分类器类
                 CascadeClassifier face_cascade = new CascadeClassifier(@"D:\sourcecodes\METU\METU.Win\haarcascades\haarcascade_frontalface_default.xml");

                //测试图片
                Mat image = Cv2.ImRead(@"D:\sourcecodes\METU\METU.Win\FaceDetection\1.jpg");

                //灰度图片
                Mat newImage = new Mat();

                // 转换为灰度图
                Cv2.CvtColor(image, newImage, ColorConversionCodes.BGR2GRAY);

                // 调用人脸检测器检测
                var faces = face_cascade.DetectMultiScale(newImage, 1.3, 10, HaarDetectionTypes.DoCannyPruning);

                if (faces.Length < 1)
                    Console.WriteLine($"没有检测到人脸");

                if (faces.Length > 1)
                    Console.WriteLine($"检测到多张人脸");
            

        }
    }
}
