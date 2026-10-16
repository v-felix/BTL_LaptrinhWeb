using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalManagement.Web.Migrations
{
    /// <inheritdoc />
    public partial class VietnameseCatalogAndVnd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Apply the one-time sample-data update before adding currency constraints.
            migrationBuilder.Sql("""
                -- Gia mau VND cua do an: nhan 1.000 cac so tien BDT da nhap.
                -- Day la bang gia minh hoa, khong phai ty gia thi truong.
                UPDATE [DiagnosticServices] SET [Price] = [Price] * 1000, [CurrencyCode] = N'VND' WHERE [CurrencyCode] = N'BDT';
                UPDATE [DiagnosticServices] SET [CurrencyCode] = N'VND' WHERE [CurrencyCode] <> N'VND';
                UPDATE [HealthChecks] SET [Price] = [Price] * 1000, [CurrencyCode] = N'VND' WHERE [CurrencyCode] = N'BDT';
                UPDATE [HealthChecks] SET [CurrencyCode] = N'VND' WHERE [CurrencyCode] <> N'VND';
                UPDATE [DiagnosticBookings] SET [Price] = [Price] * 1000, [CurrencyCode] = N'VND' WHERE [CurrencyCode] = N'BDT';
                UPDATE [DiagnosticBookings] SET [CurrencyCode] = N'VND' WHERE [CurrencyCode] <> N'VND';
                UPDATE [PackageBookings] SET [TotalAmount] = [TotalAmount] * 1000, [AmountPaid] = [AmountPaid] * 1000, [CurrencyCode] = N'VND' WHERE [CurrencyCode] = N'BDT';
                UPDATE [PackageBookings] SET [CurrencyCode] = N'VND' WHERE [CurrencyCode] <> N'VND';
                UPDATE [AspNetUsers] SET [FullName] = N'Nguyễn Minh Đức' WHERE [Id] = N'legacy-2' AND [FullName] = N'Dr. Xihad Ansari';
                UPDATE [Doctors] SET [Designation] = N'Bác sĩ chuyên khoa II - Nội tổng quát' WHERE [UserId] = N'legacy-2' AND [Designation] = N'MBBS, FCPS (Medicine)';
                UPDATE [Doctors] SET [About] = N'Bác sĩ chuyên khoa Nội tổng quát, phụ trách thăm khám, tư vấn và theo dõi điều trị cho người bệnh.' WHERE [UserId] = N'legacy-2' AND [About] = N'Mental health & counseling.';
                UPDATE [Doctors] SET [Speciality] = N'Nội tổng quát' WHERE [Speciality] = N'General Physician';
                UPDATE [AspNetUsers] SET [FullName] = N'Trần Quốc Huy' WHERE [Id] = N'legacy-3' AND [FullName] = N'Dr. Mahmud Hasan';
                UPDATE [Doctors] SET [Designation] = N'Thạc sĩ, Bác sĩ - Tim mạch' WHERE [UserId] = N'legacy-3' AND [Designation] = N'MBBS, MD (Cardiology)';
                UPDATE [Doctors] SET [About] = N'Bác sĩ chuyên khoa Tim mạch, phụ trách thăm khám, tư vấn và theo dõi điều trị cho người bệnh.' WHERE [UserId] = N'legacy-3' AND [About] = N'Heart care and cardiac consultation.';
                UPDATE [Doctors] SET [Speciality] = N'Tim mạch' WHERE [Speciality] = N'Cardiology';
                UPDATE [AspNetUsers] SET [FullName] = N'Lê Thu Hà' WHERE [Id] = N'legacy-4' AND [FullName] = N'Dr. Farzana Akter';
                UPDATE [Doctors] SET [Designation] = N'Bác sĩ chuyên khoa I - Thần kinh' WHERE [UserId] = N'legacy-4' AND [Designation] = N'MBBS, MD (Neurology)';
                UPDATE [Doctors] SET [About] = N'Bác sĩ chuyên khoa Thần kinh, phụ trách thăm khám, tư vấn và theo dõi điều trị cho người bệnh.' WHERE [UserId] = N'legacy-4' AND [About] = N'Neurological disorders & headache clinic.';
                UPDATE [Doctors] SET [Speciality] = N'Thần kinh' WHERE [Speciality] = N'Neurology';
                UPDATE [AspNetUsers] SET [FullName] = N'Phạm Hoàng Nam' WHERE [Id] = N'legacy-5' AND [FullName] = N'Dr. Shanto Roy';
                UPDATE [Doctors] SET [Designation] = N'Bác sĩ chuyên khoa II - Chấn thương chỉnh hình' WHERE [UserId] = N'legacy-5' AND [Designation] = N'MBBS, MS (Orthopedics)';
                UPDATE [Doctors] SET [About] = N'Bác sĩ chuyên khoa Chấn thương chỉnh hình, phụ trách thăm khám, tư vấn và theo dõi điều trị cho người bệnh.' WHERE [UserId] = N'legacy-5' AND [About] = N'Bone, joint & sports injury specialist.';
                UPDATE [Doctors] SET [Speciality] = N'Chấn thương chỉnh hình' WHERE [Speciality] = N'Orthopedics';
                UPDATE [AspNetUsers] SET [FullName] = N'Nguyễn Ngọc Lan' WHERE [Id] = N'legacy-6' AND [FullName] = N'Dr. Nabila Islam';
                UPDATE [Doctors] SET [Designation] = N'Thạc sĩ, Bác sĩ - Da liễu' WHERE [UserId] = N'legacy-6' AND [Designation] = N'MBBS, DDV (Dermatology)';
                UPDATE [Doctors] SET [About] = N'Bác sĩ chuyên khoa Da liễu, phụ trách thăm khám, tư vấn và theo dõi điều trị cho người bệnh.' WHERE [UserId] = N'legacy-6' AND [About] = N'Skin, hair & nail care.';
                UPDATE [Doctors] SET [Speciality] = N'Da liễu' WHERE [Speciality] = N'Dermatology';
                UPDATE [AspNetUsers] SET [FullName] = N'Đặng Anh Tuấn' WHERE [Id] = N'legacy-7' AND [FullName] = N'Dr. Tareq Amin';
                UPDATE [Doctors] SET [Designation] = N'Bác sĩ chuyên khoa I - Nhi khoa' WHERE [UserId] = N'legacy-7' AND [Designation] = N'MBBS, DCH, MD (Pediatrics)';
                UPDATE [Doctors] SET [About] = N'Bác sĩ chuyên khoa Nhi khoa, phụ trách thăm khám, tư vấn và theo dõi điều trị cho người bệnh.' WHERE [UserId] = N'legacy-7' AND [About] = N'Child health & vaccination.';
                UPDATE [Doctors] SET [Speciality] = N'Nhi khoa' WHERE [Speciality] = N'Pediatrics';
                UPDATE [AspNetUsers] SET [FullName] = N'Vũ Mai Phương' WHERE [Id] = N'legacy-8' AND [FullName] = N'Dr. Nusrat Jahan';
                UPDATE [Doctors] SET [Designation] = N'Bác sĩ chuyên khoa II - Sản phụ khoa' WHERE [UserId] = N'legacy-8' AND [Designation] = N'MBBS, FCPS (Gynae & Obs)';
                UPDATE [Doctors] SET [About] = N'Bác sĩ chuyên khoa Sản phụ khoa, phụ trách thăm khám, tư vấn và theo dõi điều trị cho người bệnh.' WHERE [UserId] = N'legacy-8' AND [About] = N'Women’s health & maternity care.';
                UPDATE [Doctors] SET [Speciality] = N'Sản phụ khoa' WHERE [Speciality] = N'Gynecology & Obstetrics';
                UPDATE [AspNetUsers] SET [FullName] = N'Bùi Quang Minh' WHERE [Id] = N'legacy-9' AND [FullName] = N'Dr. Imran Kabir';
                UPDATE [Doctors] SET [Designation] = N'Thạc sĩ, Bác sĩ - Tai Mũi Họng' WHERE [UserId] = N'legacy-9' AND [Designation] = N'MBBS, DLO (ENT)';
                UPDATE [Doctors] SET [About] = N'Bác sĩ chuyên khoa Tai Mũi Họng, phụ trách thăm khám, tư vấn và theo dõi điều trị cho người bệnh.' WHERE [UserId] = N'legacy-9' AND [About] = N'Ear, nose & throat diseases.';
                UPDATE [Doctors] SET [Speciality] = N'Tai Mũi Họng' WHERE [Speciality] = N'ENT';
                UPDATE [AspNetUsers] SET [FullName] = N'Đỗ Thanh Hương' WHERE [Id] = N'legacy-10' AND [FullName] = N'Dr. Sanjida Rahman';
                UPDATE [Doctors] SET [Designation] = N'Bác sĩ chuyên khoa I - Mắt' WHERE [UserId] = N'legacy-10' AND [Designation] = N'MBBS, DO (Ophthalmology)';
                UPDATE [Doctors] SET [About] = N'Bác sĩ chuyên khoa Mắt, phụ trách thăm khám, tư vấn và theo dõi điều trị cho người bệnh.' WHERE [UserId] = N'legacy-10' AND [About] = N'Eye specialist & cataract clinic.';
                UPDATE [Doctors] SET [Speciality] = N'Mắt' WHERE [Speciality] = N'Ophthalmology';
                UPDATE [AspNetUsers] SET [FullName] = N'Hoàng Việt Anh' WHERE [Id] = N'legacy-11' AND [FullName] = N'Dr. Rakibul Islam';
                UPDATE [Doctors] SET [Designation] = N'Bác sĩ chuyên khoa II - Tiêu hóa' WHERE [UserId] = N'legacy-11' AND [Designation] = N'MBBS, MD (Gastroenterology)';
                UPDATE [Doctors] SET [About] = N'Bác sĩ chuyên khoa Tiêu hóa, phụ trách thăm khám, tư vấn và theo dõi điều trị cho người bệnh.' WHERE [UserId] = N'legacy-11' AND [About] = N'Liver & digestive diseases.';
                UPDATE [Doctors] SET [Speciality] = N'Tiêu hóa' WHERE [Speciality] = N'Gastroenterology';
                UPDATE [AspNetUsers] SET [FullName] = N'Phan Thị Hạnh' WHERE [Id] = N'legacy-12' AND [FullName] = N'Dr. Shirin Akhter';
                UPDATE [Doctors] SET [Designation] = N'Thạc sĩ, Bác sĩ - Thận nội khoa' WHERE [UserId] = N'legacy-12' AND [Designation] = N'MBBS, MD (Nephrology)';
                UPDATE [Doctors] SET [About] = N'Bác sĩ chuyên khoa Thận nội khoa, phụ trách thăm khám, tư vấn và theo dõi điều trị cho người bệnh.' WHERE [UserId] = N'legacy-12' AND [About] = N'Kidney diseases & dialysis advice.';
                UPDATE [Doctors] SET [Speciality] = N'Thận nội khoa' WHERE [Speciality] = N'Nephrology';
                UPDATE [AspNetUsers] SET [FullName] = N'Võ Thành Đạt' WHERE [Id] = N'legacy-13' AND [FullName] = N'Dr. Arif Chowdhury';
                UPDATE [Doctors] SET [Designation] = N'Bác sĩ chuyên khoa I - Tiết niệu' WHERE [UserId] = N'legacy-13' AND [Designation] = N'MBBS, MS (Urology)';
                UPDATE [Doctors] SET [About] = N'Bác sĩ chuyên khoa Tiết niệu, phụ trách thăm khám, tư vấn và theo dõi điều trị cho người bệnh.' WHERE [UserId] = N'legacy-13' AND [About] = N'Urinary & male reproductive health.';
                UPDATE [Doctors] SET [Speciality] = N'Tiết niệu' WHERE [Speciality] = N'Urology';
                UPDATE [AspNetUsers] SET [FullName] = N'Dương Thùy Linh' WHERE [Id] = N'legacy-14' AND [FullName] = N'Dr. Tahmina Sultana';
                UPDATE [Doctors] SET [Designation] = N'Bác sĩ chuyên khoa II - Nội tiết' WHERE [UserId] = N'legacy-14' AND [Designation] = N'MBBS, MD (Endocrinology)';
                UPDATE [Doctors] SET [About] = N'Bác sĩ chuyên khoa Nội tiết, phụ trách thăm khám, tư vấn và theo dõi điều trị cho người bệnh.' WHERE [UserId] = N'legacy-14' AND [About] = N'Diabetes & hormone disorders.';
                UPDATE [Doctors] SET [Speciality] = N'Nội tiết' WHERE [Speciality] = N'Endocrinology';
                UPDATE [AspNetUsers] SET [FullName] = N'Ngô Trung Kiên' WHERE [Id] = N'legacy-15' AND [FullName] = N'Dr. Jubayer Ahmed';
                UPDATE [Doctors] SET [Designation] = N'Thạc sĩ, Bác sĩ - Ung bướu' WHERE [UserId] = N'legacy-15' AND [Designation] = N'MBBS, MD (Oncology)';
                UPDATE [Doctors] SET [About] = N'Bác sĩ chuyên khoa Ung bướu, phụ trách thăm khám, tư vấn và theo dõi điều trị cho người bệnh.' WHERE [UserId] = N'legacy-15' AND [About] = N'Cancer diagnosis & chemotherapy advice.';
                UPDATE [Doctors] SET [Speciality] = N'Ung bướu' WHERE [Speciality] = N'Oncology';
                UPDATE [AspNetUsers] SET [FullName] = N'Trịnh Bảo Châu' WHERE [Id] = N'legacy-16' AND [FullName] = N'Dr. Rukhsana Parvin';
                UPDATE [Doctors] SET [Designation] = N'Bác sĩ chuyên khoa I - Tâm thần' WHERE [UserId] = N'legacy-16' AND [Designation] = N'MBBS, MD (Psychiatry)';
                UPDATE [Doctors] SET [About] = N'Bác sĩ chuyên khoa Tâm thần, phụ trách thăm khám, tư vấn và theo dõi điều trị cho người bệnh.' WHERE [UserId] = N'legacy-16' AND [About] = N'Mental health & counseling.';
                UPDATE [Doctors] SET [Speciality] = N'Tâm thần' WHERE [Speciality] = N'Psychiatry';
                UPDATE [AspNetUsers] SET [FullName] = N'Đinh Hữu Phúc' WHERE [Id] = N'legacy-17' AND [FullName] = N'Dr. Morshed Alam';
                UPDATE [Doctors] SET [Designation] = N'Bác sĩ chuyên khoa II - Hô hấp' WHERE [UserId] = N'legacy-17' AND [Designation] = N'MBBS, MD (Pulmonology)';
                UPDATE [Doctors] SET [About] = N'Bác sĩ chuyên khoa Hô hấp, phụ trách thăm khám, tư vấn và theo dõi điều trị cho người bệnh.' WHERE [UserId] = N'legacy-17' AND [About] = N'Asthma, COPD & lung care.';
                UPDATE [Doctors] SET [Speciality] = N'Hô hấp' WHERE [Speciality] = N'Pulmonology';
                UPDATE [AspNetUsers] SET [FullName] = N'Hồ Khánh Linh' WHERE [Id] = N'legacy-18' AND [FullName] = N'Dr. Sumaiya Khan';
                UPDATE [Doctors] SET [Designation] = N'Thạc sĩ, Bác sĩ - Cơ xương khớp' WHERE [UserId] = N'legacy-18' AND [Designation] = N'MBBS, MD (Rheumatology)';
                UPDATE [Doctors] SET [About] = N'Bác sĩ chuyên khoa Cơ xương khớp, phụ trách thăm khám, tư vấn và theo dõi điều trị cho người bệnh.' WHERE [UserId] = N'legacy-18' AND [About] = N'Arthritis & autoimmune diseases.';
                UPDATE [Doctors] SET [Speciality] = N'Cơ xương khớp' WHERE [Speciality] = N'Rheumatology';
                UPDATE [AspNetUsers] SET [FullName] = N'Lý Đức Thịnh' WHERE [Id] = N'legacy-19' AND [FullName] = N'Dr. Nafis Rahim';
                UPDATE [Doctors] SET [Designation] = N'Bác sĩ chuyên khoa I - Huyết học' WHERE [UserId] = N'legacy-19' AND [Designation] = N'MBBS, MD (Hematology)';
                UPDATE [Doctors] SET [About] = N'Bác sĩ chuyên khoa Huyết học, phụ trách thăm khám, tư vấn và theo dõi điều trị cho người bệnh.' WHERE [UserId] = N'legacy-19' AND [About] = N'Blood disorders & anemia clinic.';
                UPDATE [Doctors] SET [Speciality] = N'Huyết học' WHERE [Speciality] = N'Hematology';
                UPDATE [AspNetUsers] SET [FullName] = N'Mai Hải Yến' WHERE [Id] = N'legacy-20' AND [FullName] = N'Dr. Tasnim Noor';
                UPDATE [Doctors] SET [Designation] = N'Bác sĩ chuyên khoa II - Răng Hàm Mặt' WHERE [UserId] = N'legacy-20' AND [Designation] = N'BDS, FCPS (Oral & Maxillofacial Surgery)';
                UPDATE [Doctors] SET [About] = N'Bác sĩ chuyên khoa Răng Hàm Mặt, phụ trách thăm khám, tư vấn và theo dõi điều trị cho người bệnh.' WHERE [UserId] = N'legacy-20' AND [About] = N'Oral surgery & dental implants.';
                UPDATE [Doctors] SET [Speciality] = N'Răng Hàm Mặt' WHERE [Speciality] = N'Dental Surgery';
                UPDATE [AspNetUsers] SET [FullName] = N'Tạ Minh Quân' WHERE [Id] = N'legacy-21' AND [FullName] = N'Dr. Reaz Ahmed';
                UPDATE [Doctors] SET [Designation] = N'Thạc sĩ, Bác sĩ - Gây mê hồi sức' WHERE [UserId] = N'legacy-21' AND [Designation] = N'MBBS, MD (Anesthesiology)';
                UPDATE [Doctors] SET [About] = N'Bác sĩ chuyên khoa Gây mê hồi sức, phụ trách thăm khám, tư vấn và theo dõi điều trị cho người bệnh.' WHERE [UserId] = N'legacy-21' AND [About] = N'Perioperative & pain management.';
                UPDATE [Doctors] SET [Speciality] = N'Gây mê hồi sức' WHERE [Speciality] = N'Anesthesiology';
                UPDATE [AspNetUsers] SET [FullName] = N'Cao Ngọc Ánh' WHERE [Id] = N'legacy-22' AND [FullName] = N'Dr. Lamia Hossain';
                UPDATE [Doctors] SET [Designation] = N'Bác sĩ chuyên khoa I - Phục hồi chức năng' WHERE [UserId] = N'legacy-22' AND [Designation] = N'MBBS, MD (PM&R)';
                UPDATE [Doctors] SET [About] = N'Bác sĩ chuyên khoa Phục hồi chức năng, phụ trách thăm khám, tư vấn và theo dõi điều trị cho người bệnh.' WHERE [UserId] = N'legacy-22' AND [About] = N'Rehab, physio & pain clinic.';
                UPDATE [Doctors] SET [Speciality] = N'Phục hồi chức năng' WHERE [Speciality] = N'Physical Medicine & Rehabilitation';
                UPDATE [DiagnosticServices] SET [Name] = N'Xét nghiệm máu tổng quát', [Description] = N'Phân tích các chỉ số máu trong gói xét nghiệm tổng quát.' WHERE [Id] = 1 AND [Name] = N'Blood Test';
                UPDATE [DiagnosticServices] SET [Name] = N'Chụp X-quang ngực', [Description] = N'Chụp X-quang vùng ngực theo chỉ định của bác sĩ.' WHERE [Id] = 2 AND [Name] = N'X-Ray Chest';
                UPDATE [DiagnosticServices] SET [Name] = N'Tổng phân tích nước tiểu', [Description] = N'Phân tích các thành phần trong mẫu nước tiểu.' WHERE [Id] = 3 AND [Name] = N'Urine Test';
                UPDATE [DiagnosticServices] SET [Name] = N'Xét nghiệm phân', [Description] = N'Phân tích mẫu phân theo yêu cầu xét nghiệm.' WHERE [Id] = 4 AND [Name] = N'Stool Test';
                UPDATE [DiagnosticServices] SET [Name] = N'Chụp cộng hưởng từ não', [Description] = N'Chụp cộng hưởng từ vùng não theo chỉ định.' WHERE [Id] = 5 AND [Name] = N'MRI Brain';
                UPDATE [DiagnosticServices] SET [Name] = N'Điện tâm đồ', [Description] = N'Ghi nhận hoạt động điện của tim.' WHERE [Id] = 6 AND [Name] = N'ECG';
                UPDATE [DiagnosticServices] SET [Name] = N'Siêu âm ổ bụng', [Description] = N'Khảo sát các cơ quan trong ổ bụng bằng siêu âm.' WHERE [Id] = 7 AND [Name] = N'Ultrasound Abdomen';
                UPDATE [DiagnosticServices] SET [Name] = N'Xét nghiệm chức năng tuyến giáp', [Description] = N'Định lượng các chỉ số đánh giá chức năng tuyến giáp.' WHERE [Id] = 8 AND [Name] = N'Thyroid Function Test';
                UPDATE [DiagnosticServices] SET [Name] = N'Chụp cắt lớp vi tính ngực', [Description] = N'Chụp cắt lớp vi tính vùng ngực.' WHERE [Id] = 9 AND [Name] = N'CT Scan Chest';
                UPDATE [DiagnosticServices] SET [Name] = N'Xét nghiệm mỡ máu', [Description] = N'Định lượng cholesterol và các thành phần lipid máu.' WHERE [Id] = 10 AND [Name] = N'Lipid Profile';
                UPDATE [DiagnosticServices] SET [Name] = N'Chụp nhũ ảnh', [Description] = N'Chụp X-quang tuyến vú theo chỉ định.' WHERE [Id] = 11 AND [Name] = N'Mammography';
                UPDATE [DiagnosticServices] SET [Name] = N'Xét nghiệm chức năng gan', [Description] = N'Phân tích các chỉ số đánh giá chức năng gan.' WHERE [Id] = 12 AND [Name] = N'Liver Function Test';
                UPDATE [DiagnosticServices] SET [Name] = N'Đo mật độ xương', [Description] = N'Khảo sát mật độ khoáng của xương.' WHERE [Id] = 13 AND [Name] = N'Bone Density Scan';
                UPDATE [DiagnosticServices] SET [Name] = N'Định lượng huyết sắc tố', [Description] = N'Định lượng hemoglobin trong máu.' WHERE [Id] = 14 AND [Name] = N'Hemoglobin Test';
                UPDATE [DiagnosticServices] SET [Name] = N'Siêu âm tim', [Description] = N'Khảo sát cấu trúc và hoạt động của tim bằng siêu âm.' WHERE [Id] = 15 AND [Name] = N'Echo Cardiogram';
                UPDATE [DiagnosticServices] SET [Name] = N'Nghiệm pháp dung nạp glucose', [Description] = N'Đo đường huyết theo quy trình nghiệm pháp dung nạp glucose.' WHERE [Id] = 16 AND [Name] = N'Glucose Tolerance Test';
                UPDATE [DiagnosticServices] SET [Name] = N'Chụp PET', [Description] = N'Chụp hình ảnh chuyển hóa bằng kỹ thuật PET theo chỉ định.' WHERE [Id] = 17 AND [Name] = N'PET Scan';
                UPDATE [DiagnosticServices] SET [Name] = N'Định lượng vitamin D', [Description] = N'Định lượng vitamin D trong mẫu máu.' WHERE [Id] = 18 AND [Name] = N'Vitamin D Test';
                UPDATE [DiagnosticServices] SET [Name] = N'Nội soi đại tràng', [Description] = N'Thực hiện nội soi kiểm tra đại tràng theo chỉ định.' WHERE [Id] = 19 AND [Name] = N'Colonoscopy Preparation';
                UPDATE [DiagnosticServices] SET [Name] = N'Xét nghiệm dị ứng', [Description] = N'Xét nghiệm các chỉ số dị ứng theo danh mục dịch vụ.' WHERE [Id] = 20 AND [Name] = N'Allergy Test';
                UPDATE [DiagnosticServices] SET [Name] = N'Tổng phân tích tế bào máu', [Description] = N'Đếm và phân tích hồng cầu, bạch cầu, tiểu cầu.' WHERE [Id] = 21 AND [Name] = N'Complete Blood Count (CBC)';
                UPDATE [DiagnosticServices] SET [Name] = N'Xét nghiệm chức năng thận', [Description] = N'Phân tích các chỉ số đánh giá chức năng thận.' WHERE [Id] = 22 AND [Name] = N'Kidney Function Test';
                UPDATE [DiagnosticServices] SET [Name] = N'Định lượng HbA1c', [Description] = N'Định lượng HbA1c trong mẫu máu.' WHERE [Id] = 23 AND [Name] = N'HbA1c Test';
                UPDATE [DiagnosticServices] SET [Name] = N'Định lượng PSA', [Description] = N'Định lượng kháng nguyên đặc hiệu tuyến tiền liệt.' WHERE [Id] = 24 AND [Name] = N'PSA Test';
                UPDATE [DiagnosticServices] SET [Name] = N'Xét nghiệm tế bào cổ tử cung', [Description] = N'Phân tích tế bào từ mẫu cổ tử cung.' WHERE [Id] = 25 AND [Name] = N'Pap Smear';
                UPDATE [DiagnosticServices] SET [Name] = N'Xét nghiệm tốc độ máu lắng', [Description] = N'Đo tốc độ lắng của hồng cầu.' WHERE [Id] = 26 AND [Name] = N'ESR Test';
                UPDATE [DiagnosticServices] SET [Name] = N'Định lượng vitamin B12', [Description] = N'Định lượng vitamin B12 trong mẫu máu.' WHERE [Id] = 27 AND [Name] = N'Vitamin B12 Test';
                UPDATE [DiagnosticServices] SET [Name] = N'Định lượng sắt huyết thanh', [Description] = N'Định lượng sắt trong huyết thanh.' WHERE [Id] = 28 AND [Name] = N'Iron Test';
                UPDATE [DiagnosticServices] SET [Name] = N'Sinh thiết tủy xương', [Description] = N'Lấy và phân tích mẫu tủy xương theo chỉ định.' WHERE [Id] = 29 AND [Name] = N'Bone Marrow Biopsy';
                UPDATE [DiagnosticServices] SET [Name] = N'Chụp mạch máu', [Description] = N'Khảo sát hình ảnh mạch máu theo chỉ định.' WHERE [Id] = 30 AND [Name] = N'Angiography';
                UPDATE [DiagnosticServices] SET [Name] = N'Đo mật độ xương DEXA', [Description] = N'Đo mật độ khoáng xương bằng kỹ thuật DEXA.' WHERE [Id] = 31 AND [Name] = N'DEXA Scan';
                UPDATE [DiagnosticServices] SET [Name] = N'Theo dõi điện tâm đồ Holter', [Description] = N'Ghi nhận điện tim liên tục bằng thiết bị Holter.' WHERE [Id] = 32 AND [Name] = N'Holter Monitor';
                UPDATE [DiagnosticServices] SET [Name] = N'Điện tâm đồ gắng sức', [Description] = N'Ghi nhận điện tim trong quá trình thực hiện nghiệm pháp gắng sức.' WHERE [Id] = 33 AND [Name] = N'Stress Test';
                UPDATE [DiagnosticServices] SET [Name] = N'Thông tim chẩn đoán', [Description] = N'Thực hiện thông tim phục vụ chẩn đoán theo chỉ định.' WHERE [Id] = 34 AND [Name] = N'Cardiac Catheterization';
                UPDATE [DiagnosticServices] SET [Name] = N'Sinh thiết mô', [Description] = N'Lấy mẫu mô và chuyển phân tích giải phẫu bệnh.' WHERE [Id] = 35 AND [Name] = N'Biopsy';
                UPDATE [DiagnosticServices] SET [Name] = N'Chọc hút tế bào bằng kim nhỏ', [Description] = N'Lấy mẫu tế bào bằng kỹ thuật chọc hút kim nhỏ.' WHERE [Id] = 36 AND [Name] = N'Fine Needle Aspiration';
                UPDATE [DiagnosticServices] SET [Name] = N'Điện não đồ', [Description] = N'Ghi nhận hoạt động điện của não.' WHERE [Id] = 37 AND [Name] = N'EEG';
                UPDATE [DiagnosticServices] SET [Name] = N'Điện cơ', [Description] = N'Ghi nhận hoạt động điện của cơ và dây thần kinh.' WHERE [Id] = 38 AND [Name] = N'EMG';
                UPDATE [DiagnosticServices] SET [Name] = N'Nội soi tiêu hóa', [Description] = N'Khảo sát đường tiêu hóa bằng nội soi.' WHERE [Id] = 39 AND [Name] = N'Endoscopy';
                UPDATE [DiagnosticServices] SET [Name] = N'Nội soi phế quản', [Description] = N'Khảo sát đường thở và phế quản bằng nội soi.' WHERE [Id] = 40 AND [Name] = N'Bronchoscopy';
                UPDATE [DiagnosticServices] SET [Name] = N'Nội soi bàng quang', [Description] = N'Khảo sát bàng quang và niệu đạo bằng nội soi.' WHERE [Id] = 41 AND [Name] = N'Cystoscopy';
                UPDATE [DiagnosticServices] SET [Name] = N'Đo chức năng hô hấp', [Description] = N'Đo các thông số thông khí của phổi.' WHERE [Id] = 42 AND [Name] = N'Spirometry';
                UPDATE [DiagnosticServices] SET [Name] = N'Đo thính lực', [Description] = N'Đo khả năng nghe theo quy trình thính lực.' WHERE [Id] = 43 AND [Name] = N'Audiometry';
                UPDATE [DiagnosticServices] SET [Category] = N'Xét nghiệm' WHERE [Category] = N'Laboratory';
                UPDATE [DiagnosticServices] SET [Category] = N'Chẩn đoán hình ảnh' WHERE [Category] = N'Radiology';
                UPDATE [DiagnosticServices] SET [Category] = N'Tim mạch' WHERE [Category] = N'Cardiology';
                UPDATE [DiagnosticServices] SET [Category] = N'Giải phẫu bệnh' WHERE [Category] = N'Pathology';
                UPDATE [DiagnosticServices] SET [Category] = N'Thần kinh' WHERE [Category] = N'Neurology';
                UPDATE [DiagnosticServices] SET [Category] = N'Tiêu hóa' WHERE [Category] = N'Gastroenterology';
                UPDATE [DiagnosticServices] SET [Category] = N'Hô hấp' WHERE [Category] = N'Pulmonology';
                UPDATE [DiagnosticServices] SET [Category] = N'Tiết niệu' WHERE [Category] = N'Urology';
                UPDATE [DiagnosticServices] SET [Category] = N'Tai Mũi Họng' WHERE [Category] = N'Otolaryngology';
                UPDATE [DiagnosticServices] SET [SampleType] = N'Máu' WHERE [SampleType] = N'Blood';
                UPDATE [DiagnosticServices] SET [SampleType] = N'Nước tiểu' WHERE [SampleType] = N'Urine';
                UPDATE [DiagnosticServices] SET [SampleType] = N'Phân' WHERE [SampleType] = N'Stool';
                UPDATE [DiagnosticServices] SET [SampleType] = N'Không lấy mẫu' WHERE [SampleType] = N'N/A';
                UPDATE [DiagnosticServices] SET [SampleType] = N'Mẫu cổ tử cung' WHERE [SampleType] = N'Cervical';
                UPDATE [DiagnosticServices] SET [SampleType] = N'Tủy xương' WHERE [SampleType] = N'Bone Marrow';
                UPDATE [DiagnosticServices] SET [SampleType] = N'Mô' WHERE [SampleType] = N'Tissue';
                UPDATE [HealthChecks] SET [Name] = N'Gói khám sức khỏe cơ bản', [Features] = N'Xét nghiệm máu
                Phân tích nước tiểu
                Khám và tư vấn tổng quát
                Đánh giá chỉ số khối cơ thể' WHERE [Id] = 1 AND [Name] = N'Basic Health Check';
                UPDATE [HealthChecks] SET [Name] = N'Gói khám sức khỏe nâng cao', [Features] = N'Tổng phân tích tế bào máu
                Xét nghiệm chức năng gan
                Kiểm tra tim mạch
                Xét nghiệm đường huyết
                Khám và tư vấn với bác sĩ' WHERE [Id] = 2 AND [Name] = N'Executive Health Check';
                UPDATE [HealthChecks] SET [Name] = N'Gói khám sức khỏe toàn diện', [Features] = N'Các hạng mục của gói nâng cao
                Xét nghiệm dấu ấn ung thư theo danh mục
                Kiểm tra tim mạch chuyên sâu
                Khám sức khỏe tổng quát
                Tư vấn cùng bác sĩ chuyên khoa' WHERE [Id] = 3 AND [Name] = N'Comprehensive Health Check';
                """);

            migrationBuilder.AddCheckConstraint(
                name: "CK_PackageBookings_CurrencyVnd",
                table: "PackageBookings",
                sql: "[CurrencyCode] = N'VND'");

            migrationBuilder.AddCheckConstraint(
                name: "CK_HealthChecks_CurrencyVnd",
                table: "HealthChecks",
                sql: "[CurrencyCode] = N'VND'");

            migrationBuilder.AddCheckConstraint(
                name: "CK_DiagnosticServices_CurrencyVnd",
                table: "DiagnosticServices",
                sql: "[CurrencyCode] = N'VND'");

            migrationBuilder.AddCheckConstraint(
                name: "CK_DiagnosticBookings_CurrencyVnd",
                table: "DiagnosticBookings",
                sql: "[CurrencyCode] = N'VND'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Keep localized names and VND amounts when rolling back schema constraints.
            migrationBuilder.DropCheckConstraint(
                name: "CK_PackageBookings_CurrencyVnd",
                table: "PackageBookings");

            migrationBuilder.DropCheckConstraint(
                name: "CK_HealthChecks_CurrencyVnd",
                table: "HealthChecks");

            migrationBuilder.DropCheckConstraint(
                name: "CK_DiagnosticServices_CurrencyVnd",
                table: "DiagnosticServices");

            migrationBuilder.DropCheckConstraint(
                name: "CK_DiagnosticBookings_CurrencyVnd",
                table: "DiagnosticBookings");
        }
    }
}
