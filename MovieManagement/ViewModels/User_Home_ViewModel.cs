using MovieManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using static System.Net.WebRequestMethods;

namespace MovieManagement.ViewModels
{
    class User_Home_ViewModel
    {
        public ObservableCollection<Movie> Blockbuster_Movies { get; set; }
        public ObservableCollection<Movie> Primetime_Movies { get; set; }
        public ObservableCollection<Movie> Nighttime_Movies { get; set; }
        public ObservableCollection<Movie> Standard_Movies { get; set; }


        public User_Home_ViewModel() 
        {
            Blockbuster_Movies = new ObservableCollection<Movie>()
            {
                new Movie() {
                    Title = "Super Mario",
                    PublishYear = 2023,
                    PosterUrl="ms-appx:///Assets/thumbnails/mario.jpg",
                    ImdbScore=9.2,
                    Description="Câu chuyện về cuộc phiêu lưu của anh em Super Mario đến vương quốc Nấm"
                },
                new Movie() {
                    Title = "Pyramid Game",
                    PublishYear = 2024,
                    PosterUrl="ms-appx:///Assets/thumbnails/pyramid-game.jpeg",
                    ImdbScore=9.0,
                    Description="Trò chơi tàn khốc của những kẻ tấn công, nạn nhân và những người ngoài cuộc trong lớp học khi học sinh cạnh tranh để đứng đầu kim tự tháp."
                },
                new Movie() {
                    Title = "Dune: Part Two",
                    PublishYear = 2024,
                    PosterUrl="ms-appx:///Assets/thumbnails/dune-2.png",
                    ImdbScore=9.6,
                    Description="Theo dõi cuộc hành trình thần thoại của Paul Atreides khi anh hợp nhất với Chani và người Fremen trong khi đang trên đường trả thù những kẻ âm mưu đã phá hủy gia đình anh. Đối mặt với sự lựa chọn giữa tình yêu của đời mình và số phận của vũ trụ đã biết, Paul nỗ lực ngăn chặn một tương lai khủng khiếp mà chỉ mình anh có thể thấy trước."
                },
                new Movie() {
                    Title = "Wonka",
                    PublishYear = 2023,
                    PosterUrl="ms-appx:///Assets/thumbnails/wonka.jpg",
                    ImdbScore=8.9,
                    Description="Dựa trên nhân vật từ quyến sách gối đầu giường của các em nhỏ trên toàn thế giới \"Charlie và Nhà Máy Sôcôla\" và phiên bản phim điện ảnh cùng tên vào năm 2005, WONKA kể câu chuyện kỳ diệu về hành trình của nhà phát minh, ảo thuật gia và nhà sản xuất sôcôla vĩ đại nhất thế giới trở thành WILLY WONKA đáng yêu mà chúng ta biết ngày nay. Từ đạo diễn loạt phim Paddington và nhà sản xuất loạt phim chuyển thể đình đám Harry Potter, WONKA hứa hẹn sẽ là một bộ phim đầy vui nhộn và màu sắc cho khán giả dịp Lễ Giáng Sinh năm nay."
                },
                new Movie() {
                    Title = "Loki (Season 2)",
                    PublishYear = 2023,
                    PosterUrl="ms-appx:///Assets/thumbnails/loki-2.jpg",
                    ImdbScore=9.9,
                    Description="Khi Steve Rogers, Tony Stark và Scott Lang quay trở về cột mốc 2012, ngay khi trận chiến ở New York kết thúc, để “mượn tạm” quyền trượng của Loki. Nhưng một tai nạn bất ngờ xảy đến, khiến Loki nhặt được khối lặp phương Tesseract và tiện thể tẩu thoát. Cuộc trốn thoát này đã dẫn đến dòng thời gian bị rối loạn. Cục TVA – tổ chức bảo vệ tính nguyên vẹn của dòng chảy thời gian, buộc phải can thiệp, đi gô cổ ông thần này về làm việc. Tại đây, Loki có hai lựa chọn, một là giúp TVA ổn định lại thời gian, không thì bị tiêu hủy. Dĩ nhiên Loki chọn lựa chọn thứ nhất. Nhưng đây là nước đi vô cùng mạo hiểm, vì ông thần này thường lọc lừa, “lươn lẹo”, chuyên đâm lén như bản tính tự nhiên của gã."
                },
                new Movie() {
                    Title = "Loki (Season 1)",
                    PublishYear = 2023,
                    PosterUrl="ms-appx:///Assets/thumbnails/loki-1.jpg",
                    ImdbScore=9.9,
                    Description="Khi Steve Rogers, Tony Stark và Scott Lang quay trở về cột mốc 2012, ngay khi trận chiến ở New York kết thúc, để “mượn tạm” quyền trượng của Loki. Nhưng một tai nạn bất ngờ xảy đến, khiến Loki nhặt được khối lặp phương Tesseract và tiện thể tẩu thoát. Cuộc trốn thoát này đã dẫn đến dòng thời gian bị rối loạn. Cục TVA – tổ chức bảo vệ tính nguyên vẹn của dòng chảy thời gian, buộc phải can thiệp, đi gô cổ ông thần này về làm việc. Tại đây, Loki có hai lựa chọn, một là giúp TVA ổn định lại thời gian, không thì bị tiêu hủy. Dĩ nhiên Loki chọn lựa chọn thứ nhất. Nhưng đây là nước đi vô cùng mạo hiểm, vì ông thần này thường lọc lừa, “lươn lẹo”, chuyên đâm lén như bản tính tự nhiên của gã."
                },
                new Movie() {
                    Title = "Kungfu Panda 4",
                    PublishYear = 2023,
                    PosterUrl="ms-appx:///Assets/thumbnails/kungfu-panda-4.jpg",
                    ImdbScore=9.9,
                    Description="Gấu péo múa võ, Tai Liêm vô địch."
                },
                new Movie() {
                    Title = "Kungfu Panda 3",
                    PublishYear = 2023,
                    PosterUrl="ms-appx:///Assets/thumbnails/kungfu-panda-3.jpg",
                    ImdbScore=9.9,
                    Description="Gấu péo múa võ, Tai Liêm vô địch."
                },
                new Movie() {
                    Title = "Kungfu Panda 2",
                    PublishYear = 2023,
                    PosterUrl="ms-appx:///Assets/thumbnails/kungfu-panda-2.jpg",
                    ImdbScore=9.9,
                    Description="Gấu péo múa võ, Tai Liêm vô địch."
                },
                new Movie() {
                    Title = "Kungfu Panda 1",
                    PublishYear = 2023,
                    PosterUrl="ms-appx:///Assets/thumbnails/kungfu-panda-1.jpg",
                    ImdbScore=9.9,
                    Description="Gấu péo múa võ, Tai Liêm vô địch."
                },
            };

            Primetime_Movies = Blockbuster_Movies;
            Nighttime_Movies = Blockbuster_Movies;
            Standard_Movies = Blockbuster_Movies;
        
        }
    }
}
