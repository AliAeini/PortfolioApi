namespace PortfolioApi.Common;

public enum DegreeLevel
{
    HighSchoolDiploma = 1,      // دیپلم
    PreUniversity = 2,           // پیش‌دانشگاهی

    Associate = 10,              // کاردانی
    Bachelor = 11,               // کارشناسی
    Master = 12,                 // کارشناسی ارشد
    Doctorate = 13,              // دکتری
    PostDoctorate = 14,          // فوق دکتری

    TechnicalDiploma = 20,       // دیپلم فنی و حرفه‌ای
    VocationalCertificate = 21,  // گواهی مهارت

    Certificate = 30,            // گواهی‌نامه
    OnlineCourse = 31,           // دوره آنلاین
    Bootcamp = 32,               // بوت‌کمپ
    Workshop = 33,               // کارگاه

    Other = 99                   // سایر
}