USE [master]
GO
/****** Object:  Database [DbBestTyping]    Script Date: 3/19/2024 9:51:58 AM ******/
CREATE DATABASE [DbBestTyping]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'DbBestTyping', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\DbBestTyping.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'DbBestTyping_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\DbBestTyping_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [DbBestTyping] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [DbBestTyping].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [DbBestTyping] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [DbBestTyping] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [DbBestTyping] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [DbBestTyping] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [DbBestTyping] SET ARITHABORT OFF 
GO
ALTER DATABASE [DbBestTyping] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [DbBestTyping] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [DbBestTyping] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [DbBestTyping] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [DbBestTyping] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [DbBestTyping] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [DbBestTyping] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [DbBestTyping] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [DbBestTyping] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [DbBestTyping] SET  ENABLE_BROKER 
GO
ALTER DATABASE [DbBestTyping] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [DbBestTyping] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [DbBestTyping] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [DbBestTyping] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [DbBestTyping] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [DbBestTyping] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [DbBestTyping] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [DbBestTyping] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [DbBestTyping] SET  MULTI_USER 
GO
ALTER DATABASE [DbBestTyping] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [DbBestTyping] SET DB_CHAINING OFF 
GO
ALTER DATABASE [DbBestTyping] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [DbBestTyping] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [DbBestTyping] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [DbBestTyping] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [DbBestTyping] SET QUERY_STORE = OFF
GO
USE [DbBestTyping]
GO
/****** Object:  Table [dbo].[CLASSROOM]    Script Date: 3/19/2024 9:51:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CLASSROOM](
	[ClassRoomId] [int] IDENTITY(1,1) NOT NULL,
	[AvatarClassRoom] [ntext] NULL,
	[ClassName] [nvarchar](255) NULL,
	[Description] [nvarchar](max) NULL,
	[IsPrivate] [bit] NULL,
	[UserCreate] [int] NULL,
	[JoinCode] [varchar](25) NULL,
	[PassClassRoom] [varchar](25) NULL,
	[ListUserJoin] [nvarchar](max) NULL,
	[CreateDate] [bigint] NULL,
 CONSTRAINT [PK_CLASSROOM] PRIMARY KEY CLUSTERED 
(
	[ClassRoomId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[COMPETITION]    Script Date: 3/19/2024 9:51:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[COMPETITION](
	[CompetitionId] [int] IDENTITY(1,1) NOT NULL,
	[PeopleJoin] [int] NULL,
	[NumberOfTestsPerformed] [int] NULL,
	[CreateDate] [bigint] NULL,
	[JoinCode] [varchar](25) NULL,
	[LanguageId] [int] NULL,
	[ExerciseTextID] [int] NULL,
	[IsPrivate] [bit] NULL,
	[UserCreate] [int] NULL,
	[isOpen] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[CompetitionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EXERCISE]    Script Date: 3/19/2024 9:51:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EXERCISE](
	[ExerciseId] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](255) NULL,
	[Description] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[ExerciseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EXERCISELANGUAGE]    Script Date: 3/19/2024 9:51:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EXERCISELANGUAGE](
	[LanguageID] [int] IDENTITY(1,1) NOT NULL,
	[LanguageName] [nvarchar](50) NULL,
	[LanguageAvatar] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[LanguageID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EXERCISETEXT]    Script Date: 3/19/2024 9:51:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EXERCISETEXT](
	[ExerciseTextID] [int] IDENTITY(1,1) NOT NULL,
	[ExerciseID] [int] NULL,
	[Text] [nvarchar](max) NULL,
	[LanguageID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ExerciseTextID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TEXTPRACTICE]    Script Date: 3/19/2024 9:51:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TEXTPRACTICE](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserCreate] [int] NULL,
	[Text] [nvarchar](max) NULL,
	[Title] [nvarchar](255) NULL,
	[TextLength] [int] NULL,
	[PeopleIsCompleted] [int] NULL,
	[CreatedAt] [bigint] NULL,
	[LanguageID] [int] NULL,
	[JoinCode] [varchar](25) NULL,
	[IsPrivate] [bit] NULL,
	[Rating] [float] NULL,
	[ListUserLike] [nvarchar](max) NULL,
	[ListUserRating] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TYPEACCOUNT]    Script Date: 3/19/2024 9:51:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TYPEACCOUNT](
	[IdTypeAccount] [int] IDENTITY(1,1) NOT NULL,
	[NameTypeAccount] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TYPINGRESULTGAME]    Script Date: 3/19/2024 9:51:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TYPINGRESULTGAME](
	[ResultId] [int] IDENTITY(1,1) NOT NULL,
	[ExerciseId] [int] NULL,
	[UserID] [int] NULL,
	[Score] [int] NULL,
	[Score2] [int] NULL,
	[Timestamp] [bigint] NULL,
	[Score3] [int] NULL,
 CONSTRAINT [PK_TYPINGRESULTGAME] PRIMARY KEY CLUSTERED 
(
	[ResultId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TYPINGRESULTS]    Script Date: 3/19/2024 9:51:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TYPINGRESULTS](
	[ResultId] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[Accuracy] [float] NULL,
	[WPM] [int] NULL,
	[Mistakes] [int] NULL,
	[CorrectWords] [int] NULL,
	[TotalWords] [int] NULL,
	[Timestamp] [bigint] NULL,
	[ExerciseTextID] [int] NULL,
	[KeyStrokes] [int] NULL,
	[JoinCode] [varchar](25) NULL,
PRIMARY KEY CLUSTERED 
(
	[ResultId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[USER]    Script Date: 3/19/2024 9:51:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[USER](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[HoTen] [nvarchar](50) NULL,
	[TaiKhoan] [varchar](50) NULL,
	[MatKhau] [varchar](255) NULL,
	[Email] [varchar](50) NULL,
	[Avatar] [ntext] NULL,
	[DiaChi] [nvarchar](50) NULL,
	[DienThoai] [varchar](10) NULL,
	[XacMinh] [varchar](6) NULL,
	[LinkVerification] [nvarchar](255) NULL,
	[IsEnable] [bit] NULL,
	[GioiThieu] [nvarchar](max) NULL,
	[CreateDate] [bigint] NULL,
	[TypeAccount] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[USERPROGESS]    Script Date: 3/19/2024 9:51:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[USERPROGESS](
	[ProgressID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[SoTuDaGo] [int] NULL,
	[SoBaiKiemTra] [int] NULL,
	[CuocThiThamGia] [int] NULL,
	[WPMTotNhat] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ProgressID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[CLASSROOM] ON 

INSERT [dbo].[CLASSROOM] ([ClassRoomId], [AvatarClassRoom], [ClassName], [Description], [IsPrivate], [UserCreate], [JoinCode], [PassClassRoom], [ListUserJoin], [CreateDate]) VALUES (1, N'', N'Lớp 12a3', N'Lớp của thầy Tuấn', 1, 1, NULL, NULL, N'[{"UserId":1,"Email":"pingvocuc333@gmail.com","DateJoin":1710645800423},{"UserId":2,"Email":"pingvocuc222@gmail.com","DateJoin":1710645800423}]', 1710645777607)
SET IDENTITY_INSERT [dbo].[CLASSROOM] OFF
GO
SET IDENTITY_INSERT [dbo].[COMPETITION] ON 

INSERT [dbo].[COMPETITION] ([CompetitionId], [PeopleJoin], [NumberOfTestsPerformed], [CreateDate], [JoinCode], [LanguageId], [ExerciseTextID], [IsPrivate], [UserCreate], [isOpen]) VALUES (1, 0, 0, 1708710787004, N'eJPuewwL2KK', 19, 21, 0, 1, 0)
INSERT [dbo].[COMPETITION] ([CompetitionId], [PeopleJoin], [NumberOfTestsPerformed], [CreateDate], [JoinCode], [LanguageId], [ExerciseTextID], [IsPrivate], [UserCreate], [isOpen]) VALUES (2, 0, 0, 1708780162328, N'MFBqw1BtX6C', 19, 21, 0, 1, 1)
INSERT [dbo].[COMPETITION] ([CompetitionId], [PeopleJoin], [NumberOfTestsPerformed], [CreateDate], [JoinCode], [LanguageId], [ExerciseTextID], [IsPrivate], [UserCreate], [isOpen]) VALUES (3, 2, 3, 1708780244598, N'0XSKr08uNME', 19, 22, 0, 1, 1)
INSERT [dbo].[COMPETITION] ([CompetitionId], [PeopleJoin], [NumberOfTestsPerformed], [CreateDate], [JoinCode], [LanguageId], [ExerciseTextID], [IsPrivate], [UserCreate], [isOpen]) VALUES (4, 1, 2, 1709080530796, N'Rc1sN8NN1yF', 19, 21, 0, 1, 1)
INSERT [dbo].[COMPETITION] ([CompetitionId], [PeopleJoin], [NumberOfTestsPerformed], [CreateDate], [JoinCode], [LanguageId], [ExerciseTextID], [IsPrivate], [UserCreate], [isOpen]) VALUES (5, 0, 0, 1709694148916, N'XRkPcWca9y9', 19, 21, 0, 1, 1)
INSERT [dbo].[COMPETITION] ([CompetitionId], [PeopleJoin], [NumberOfTestsPerformed], [CreateDate], [JoinCode], [LanguageId], [ExerciseTextID], [IsPrivate], [UserCreate], [isOpen]) VALUES (6, 1, 1, 1710260306622, N'FOLulW3Ewvd', 19, 21, 0, 1, 1)
SET IDENTITY_INSERT [dbo].[COMPETITION] OFF
GO
SET IDENTITY_INSERT [dbo].[EXERCISE] ON 

INSERT [dbo].[EXERCISE] ([ExerciseId], [Title], [Description]) VALUES (1, N'Kiểm tra tốc độ đánh máy bình thường', N'Top 200 từ')
INSERT [dbo].[EXERCISE] ([ExerciseId], [Title], [Description]) VALUES (2, N'Kiểm tra tốc độ đánh máy nâng cao', N'Top 1000 từ')
INSERT [dbo].[EXERCISE] ([ExerciseId], [Title], [Description]) VALUES (3, N'Chế độ đánh máy tùy chỉnh', N'Tạo đề riêng của bạn')
INSERT [dbo].[EXERCISE] ([ExerciseId], [Title], [Description]) VALUES (4, N'Cuộc thi đánh máy', N'Chơi cùng người chơi khác')
INSERT [dbo].[EXERCISE] ([ExerciseId], [Title], [Description]) VALUES (5, N'Chế độ tập luyện', N'Tập luyện theo văn bản của bạn')
INSERT [dbo].[EXERCISE] ([ExerciseId], [Title], [Description]) VALUES (6, N'Game typing 1', N'Game gõ phím 1')
SET IDENTITY_INSERT [dbo].[EXERCISE] OFF
GO
SET IDENTITY_INSERT [dbo].[EXERCISELANGUAGE] ON 

INSERT [dbo].[EXERCISELANGUAGE] ([LanguageID], [LanguageName], [LanguageAvatar]) VALUES (1, N'Arabic', N'https://typingtop.com/app-assets/images/flags/flag-21.png')
INSERT [dbo].[EXERCISELANGUAGE] ([LanguageID], [LanguageName], [LanguageAvatar]) VALUES (2, N'Chinese', N'https://typingtop.com/app-assets/images/flags/flag-49.png')
INSERT [dbo].[EXERCISELANGUAGE] ([LanguageID], [LanguageName], [LanguageAvatar]) VALUES (3, N'Danish', N'https://typingtop.com/app-assets/images/flags/flag-47.png')
INSERT [dbo].[EXERCISELANGUAGE] ([LanguageID], [LanguageName], [LanguageAvatar]) VALUES (4, N'English', N'https://typingtop.com/app-assets/images/flags/flag-2.png')
INSERT [dbo].[EXERCISELANGUAGE] ([LanguageID], [LanguageName], [LanguageAvatar]) VALUES (5, N'French', N'https://typingtop.com/app-assets/images/flags/flag-7.png')
INSERT [dbo].[EXERCISELANGUAGE] ([LanguageID], [LanguageName], [LanguageAvatar]) VALUES (6, N'German', N'	https://typingtop.com/app-assets/images/flags/flag-6.png')
INSERT [dbo].[EXERCISELANGUAGE] ([LanguageID], [LanguageName], [LanguageAvatar]) VALUES (7, N'Hebrew', N'https://typingtop.com/app-assets/images/flags/flag-38.png
')
INSERT [dbo].[EXERCISELANGUAGE] ([LanguageID], [LanguageName], [LanguageAvatar]) VALUES (8, N'Italian', N'https://typingtop.com/app-assets/images/flags/flag-18.png
')
INSERT [dbo].[EXERCISELANGUAGE] ([LanguageID], [LanguageName], [LanguageAvatar]) VALUES (9, N'Japanese', N'https://typingtop.com/app-assets/images/flags/flag-9.png')
INSERT [dbo].[EXERCISELANGUAGE] ([LanguageID], [LanguageName], [LanguageAvatar]) VALUES (10, N'Korean', N'https://typingtop.com/app-assets/images/flags/flag-10.png')
INSERT [dbo].[EXERCISELANGUAGE] ([LanguageID], [LanguageName], [LanguageAvatar]) VALUES (12, N'Norwegian', N'https://typingtop.com/app-assets/images/flags/flag-29.png')
INSERT [dbo].[EXERCISELANGUAGE] ([LanguageID], [LanguageName], [LanguageAvatar]) VALUES (13, N'Portuguese', N'https://typingtop.com/app-assets/images/flags/flag-4.png
')
INSERT [dbo].[EXERCISELANGUAGE] ([LanguageID], [LanguageName], [LanguageAvatar]) VALUES (14, N'Russian', N'https://typingtop.com/app-assets/images/flags/flag-8.png
')
INSERT [dbo].[EXERCISELANGUAGE] ([LanguageID], [LanguageName], [LanguageAvatar]) VALUES (15, N'Spanish', N'https://typingtop.com/app-assets/images/flags/flag-5.png
')
INSERT [dbo].[EXERCISELANGUAGE] ([LanguageID], [LanguageName], [LanguageAvatar]) VALUES (16, N'Thai', N'	https://typingtop.com/app-assets/images/flags/flag-19.png
')
INSERT [dbo].[EXERCISELANGUAGE] ([LanguageID], [LanguageName], [LanguageAvatar]) VALUES (17, N'Turkish', N'https://typingtop.com/app-assets/images/flags/flag-20.png
')
INSERT [dbo].[EXERCISELANGUAGE] ([LanguageID], [LanguageName], [LanguageAvatar]) VALUES (18, N'Ukrainian', N'https://typingtop.com/app-assets/images/flags/flag-13.png')
INSERT [dbo].[EXERCISELANGUAGE] ([LanguageID], [LanguageName], [LanguageAvatar]) VALUES (19, N'Vietnamese', N'https://typingtop.com/app-assets/images/flags/flag-1.png')
SET IDENTITY_INSERT [dbo].[EXERCISELANGUAGE] OFF
GO
SET IDENTITY_INSERT [dbo].[EXERCISETEXT] ON 

INSERT [dbo].[EXERCISETEXT] ([ExerciseTextID], [ExerciseID], [Text], [LanguageID]) VALUES (1, 1, N'du lịch là một hoạt động thú vị và hấp dẫn mà mọi người đều mong muốn trải nghiệm ít nhất một lần trong đời khám phá những điểm đến mới những vùng đất lạ thường mang lại cho chúng ta những trải nghiệm tuyệt vời và những kỷ niệm đáng nhớ việc lên kế hoạch cho một chuyến du lịch đòi hỏi sự chuẩn bị kỹ lưỡng từ việc tìm hiểu về địa điểm đến đặt vé máy bay chỗ ở đến việc lên lịch trình tham quan và trải nghiệm các hoạt động vui chơi mỗi điểm đến đều mang một nét đặc trưng riêng từ những bãi biển tuyệt đẹp những ngọn núi hùng vĩ đến những thành phố sôi động và nhộn nhịp du lịch còn là cơ hội để khám phá văn hóa lịch sử và ẩm thực đặc trưng của mỗi vùng miền dù là đi du lịch một mình cùng bạn bè hoặc gia đình mỗi chuyến đi luôn để lại trong lòng chúng ta những trải nghiệm đáng nhớ và những kỷ niệm không thể nào quên', 19)
INSERT [dbo].[EXERCISETEXT] ([ExerciseTextID], [ExerciseID], [Text], [LanguageID]) VALUES (2, 1, N'âm nhạc là một phần không thể thiếu trong cuộc sống hàng ngày của chúng ta nó là nguồn cảm hứng và niềm vui cho mỗi người từ những bản nhạc sôi động nhịp nhàng đến những giai điệu dịu dàng lãng mạn âm nhạc có thể thay đổi tâm trạng và tạo ra một không gian tinh thần thoải mái thư giãn từ những nghệ sĩ tài năng biểu diễn trên sân khấu đến những người yêu âm nhạc tận hưởng và chia sẻ cùng nhau âm nhạc cũng là một phần của văn hóa và truyền thống của mỗi quốc gia mỗi dân tộc từ những bài hát dân ca đến những giai điệu hiện đại âm nhạc góp phần làm phong phú thêm đời sống văn hóa và nghệ thuật không gian âm nhạc không chỉ là nơi để thưởng thức âm nhạc mà còn là nơi để kết nối và tạo ra những kỷ niệm đáng nhớ', 19)
INSERT [dbo].[EXERCISETEXT] ([ExerciseTextID], [ExerciseID], [Text], [LanguageID]) VALUES (3, 1, N'ẩm thực là một phần quan trọng của văn hóa và đời sống hàng ngày của mỗi quốc gia từ những món ăn đơn giản hàng ngày đến những món ăn cao cấp trong các nhà hàng sang trọng ẩm thực đều mang lại cho chúng ta niềm vui và trải nghiệm đặc biệt mỗi quốc gia có những đặc sản riêng biệt và phong vị độc đáo của mình từ hương vị đậm đà của ẩm thực á đông đến sự tinh tế của ẩm thực âu mỹ việc thưởng thức và khám phá các món ăn mới cũng là một cách để hiểu biết về văn hóa và truyền thống của mỗi dân tộc không chỉ là một nhu cầu cơ bản của con người ẩm thực còn là một loại hình nghệ thuật mỗi món ăn được chế biến và trình bày một cách tinh tế để làm cho bữa ăn trở nên hấp dẫn hơn thưởng thức ẩm thực cũng là cơ hội để kết nối và chia sẻ niềm vui với gia đình và bạn bè', 19)
INSERT [dbo].[EXERCISETEXT] ([ExerciseTextID], [ExerciseID], [Text], [LanguageID]) VALUES (4, 1, N'culinary is an important part of the culture and daily life of each country from simple everyday dishes to high-end cuisine in luxurious restaurants culinary brings us joy and special experiences each country has its own unique specialties and flavors from the rich taste of asian cuisine to the sophistication of european and american cuisine enjoying and exploring new dishes is also a way to understand the culture and tradition of each ethnic group not only a basic need of humans culinary is also an art form each dish is prepared and presented in a delicate way to make the meal more attractive enjoying culinary delights is also an opportunity to connect and share joy with family and friends', 4)
INSERT [dbo].[EXERCISETEXT] ([ExerciseTextID], [ExerciseID], [Text], [LanguageID]) VALUES (5, 1, N'in a test done in the students at the university of wyoming said what they considered attractive in their partners their answers were not new men tended to prefer blondes blue eyes and light skin color while women liked darker men however there were some surprises few men liked very large breasts or boyish women almost none of the women liked very muscular men in fact both men and women prefer the average too short too tall too pale or too dark were not chosen averageness still wins in a recent study scientists chose 94 faces of american women and used the computer to make a picture of an average face then they asked people which face they liked of the 94 real faces only  faces were considered to be more attractive than the average face most people said that they preferred the average face', 4)
INSERT [dbo].[EXERCISETEXT] ([ExerciseTextID], [ExerciseID], [Text], [LanguageID]) VALUES (6, 1, N'pets are domesticated animals that we keep for pleasure of all the pets i like dog the most in my house we have two dogs one is an alsatian dog and the other is a bulldog the alsatian was bought locally by my mother and the bulldog was given to us by a european family we call the alsatian ‘jimmie’ and the bulldog jonnie the dogs respond well whenever i call them by their names the alsatian is kept mainly for watching our house especially at night it barks loudly whenever there is a noise around our house my favorite is the bulldog jonnie it is a funny looking breed and rare in the world i feed him bones meat and dog biscuits i bath him everyday to keep him clean it follows me wherever i go it knows the time i usually return from school and will wait at the gate to welcome me home', 4)
INSERT [dbo].[EXERCISETEXT] ([ExerciseTextID], [ExerciseID], [Text], [LanguageID]) VALUES (7, 1, N'我 你 他 她 它 我们 你们 他们 它们 这 那 这个 那个 什么 谁 哪里 怎么 为什么 因为 如果 所以 现在 昨天 明天 今天 早上 下午 晚上 天气 天空 太阳 月亮 星星 风 雨 雪 云 雷电 彩虹 地球 大陆 海洋 山 山脚 山顶 河流 湖泊 花树 草地 花园 园林 公园 乡村 城市 街道 房屋 家庭 房间 门 窗户 地板 天花板 墙壁 书桌 椅子 床 柜子 电视 电脑 手机 建筑 桥梁 铁路 高速公路 公交车 出租车 自行车 电梯 地铁 火车站 机场 港口 学校 大学 学生 教室 老师', 2)
INSERT [dbo].[EXERCISETEXT] ([ExerciseTextID], [ExerciseID], [Text], [LanguageID]) VALUES (8, 1, N'わたし あなた 彼 彼女 それ わたしたち あなたたち 彼ら それら これ それ この その どれ だれ どこ どうして なぜ なぜなら もし ゆえに いま きのう あした きょう あさ ひる ばんてん てんくう たいよう つき ほし かぜ あめ ゆき くも かみなり にじ ちきゅう たいりく かいよう やま さんぢゅう さんけつ こうがい こうつう ぎょうとり しゃどう ふろん かくじ かしつ ゆか てんじょう かべ つくえ いす べっど つくえ でんし けいたい たてもの きょうたい てつろ ようどう こうそくどうろ こうつうき タクシー じてんしゃ でんりょく ちず ちかてつ えき くうこう こうこう がんこう がいこう がっこう だいがく がくせい きょうしつ せんせい', 9)
INSERT [dbo].[EXERCISETEXT] ([ExerciseTextID], [ExerciseID], [Text], [LanguageID]) VALUES (9, 2, N'du lịch là một hoạt động thú vị và hấp dẫn mà mọi người đều mong muốn trải nghiệm ít nhất một lần trong đời khám phá những điểm đến mới những vùng đất lạ thường mang lại cho chúng ta những trải nghiệm tuyệt vời và những kỷ niệm đáng nhớ việc lên kế hoạch cho một chuyến du lịch đòi hỏi sự chuẩn bị kỹ lưỡng từ việc tìm hiểu về địa điểm đến đặt vé máy bay chỗ ở đến việc lên lịch trình tham quan và trải nghiệm các hoạt động vui chơi mỗi điểm đến đều mang một nét đặc trưng riêng từ những bãi biển tuyệt đẹp những ngọn núi hùng vĩ đến những thành phố sôi động và nhộn nhịp du lịch còn là cơ hội để khám phá văn hóa lịch sử và ẩm thực đặc trưng của mỗi vùng miền dù là đi du lịch một mình cùng bạn bè hoặc gia đình mỗi chuyến đi luôn để lại trong lòng chúng ta những trải nghiệm đáng nhớ và những kỷ niệm không thể nào quên', 19)
INSERT [dbo].[EXERCISETEXT] ([ExerciseTextID], [ExerciseID], [Text], [LanguageID]) VALUES (10, 2, N'âm nhạc là một phần không thể thiếu trong cuộc sống hàng ngày của chúng ta nó là nguồn cảm hứng và niềm vui cho mỗi người từ những bản nhạc sôi động nhịp nhàng đến những giai điệu dịu dàng lãng mạn âm nhạc có thể thay đổi tâm trạng và tạo ra một không gian tinh thần thoải mái thư giãn từ những nghệ sĩ tài năng biểu diễn trên sân khấu đến những người yêu âm nhạc tận hưởng và chia sẻ cùng nhau âm nhạc cũng là một phần của văn hóa và truyền thống của mỗi quốc gia mỗi dân tộc từ những bài hát dân ca đến những giai điệu hiện đại âm nhạc góp phần làm phong phú thêm đời sống văn hóa và nghệ thuật không gian âm nhạc không chỉ là nơi để thưởng thức âm nhạc mà còn là nơi để kết nối và tạo ra những kỷ niệm đáng nhớ', 19)
INSERT [dbo].[EXERCISETEXT] ([ExerciseTextID], [ExerciseID], [Text], [LanguageID]) VALUES (11, 2, N'ẩm thực là một phần quan trọng của văn hóa và đời sống hàng ngày của mỗi quốc gia từ những món ăn đơn giản hàng ngày đến những món ăn cao cấp trong các nhà hàng sang trọng ẩm thực đều mang lại cho chúng ta niềm vui và trải nghiệm đặc biệt mỗi quốc gia có những đặc sản riêng biệt và phong vị độc đáo của mình từ hương vị đậm đà của ẩm thực á đông đến sự tinh tế của ẩm thực âu mỹ việc thưởng thức và khám phá các món ăn mới cũng là một cách để hiểu biết về văn hóa và truyền thống của mỗi dân tộc không chỉ là một nhu cầu cơ bản của con người ẩm thực còn là một loại hình nghệ thuật mỗi món ăn được chế biến và trình bày một cách tinh tế để làm cho bữa ăn trở nên hấp dẫn hơn thưởng thức ẩm thực cũng là cơ hội để kết nối và chia sẻ niềm vui với gia đình và bạn bè', 19)
INSERT [dbo].[EXERCISETEXT] ([ExerciseTextID], [ExerciseID], [Text], [LanguageID]) VALUES (12, 2, N'culinary is an important part of the culture and daily life of each country from simple everyday dishes to high-end cuisine in luxurious restaurants culinary brings us joy and special experiences each country has its own unique specialties and flavors from the rich taste of asian cuisine to the sophistication of european and american cuisine enjoying and exploring new dishes is also a way to understand the culture and tradition of each ethnic group not only a basic need of humans culinary is also an art form each dish is prepared and presented in a delicate way to make the meal more attractive enjoying culinary delights is also an opportunity to connect and share joy with family and friends', 4)
INSERT [dbo].[EXERCISETEXT] ([ExerciseTextID], [ExerciseID], [Text], [LanguageID]) VALUES (13, 2, N'in a test done in the students at the university of wyoming said what they considered attractive in their partners their answers were not new men tended to prefer blondes blue eyes and light skin color while women liked darker men however there were some surprises few men liked very large breasts or boyish women almost none of the women liked very muscular men in fact both men and women prefer the average too short too tall too pale or too dark were not chosen averageness still wins in a recent study scientists chose 94 faces of american women and used the computer to make a picture of an average face then they asked people which face they liked of the 94 real faces only  faces were considered to be more attractive than the average face most people said that they preferred the average face', 4)
INSERT [dbo].[EXERCISETEXT] ([ExerciseTextID], [ExerciseID], [Text], [LanguageID]) VALUES (14, 2, N'pets are domesticated animals that we keep for pleasure of all the pets i like dog the most in my house we have two dogs one is an alsatian dog and the other is a bulldog the alsatian was bought locally by my mother and the bulldog was given to us by a european family we call the alsatian ‘jimmie’ and the bulldog jonnie the dogs respond well whenever i call them by their names the alsatian is kept mainly for watching our house especially at night it barks loudly whenever there is a noise around our house my favorite is the bulldog jonnie it is a funny looking breed and rare in the world i feed him bones meat and dog biscuits i bath him everyday to keep him clean it follows me wherever i go it knows the time i usually return from school and will wait at the gate to welcome me home', 4)
INSERT [dbo].[EXERCISETEXT] ([ExerciseTextID], [ExerciseID], [Text], [LanguageID]) VALUES (15, 2, N'我 你 他 她 它 我们 你们 他们 它们 这 那 这个 那个 什么 谁 哪里 怎么 为什么 因为 如果 所以 现在 昨天 明天 今天 早上 下午 晚上 天气 天空 太阳 月亮 星星 风 雨 雪 云 雷电 彩虹 地球 大陆 海洋 山 山脚 山顶 河流 湖泊 花树 草地 花园 园林 公园 乡村 城市 街道 房屋 家庭 房间 门 窗户 地板 天花板 墙壁 书桌 椅子 床 柜子 电视 电脑 手机 建筑 桥梁 铁路 高速公路 公交车 出租车 自行车 电梯 地铁 火车站 机场 港口 学校 大学 学生 教室 老师', 2)
INSERT [dbo].[EXERCISETEXT] ([ExerciseTextID], [ExerciseID], [Text], [LanguageID]) VALUES (16, 2, N'わたし あなた 彼 彼女 それ わたしたち あなたたち 彼ら それら これ それ この その どれ だれ どこ どうして なぜ なぜなら もし ゆえに いま きのう あした きょう あさ ひる ばんてん てんくう たいよう つき ほし かぜ あめ ゆき くも かみなり にじ ちきゅう たいりく かいよう やま さんぢゅう さんけつ こうがい こうつう ぎょうとり しゃどう ふろん かくじ かしつ ゆか てんじょう かべ つくえ いす べっど つくえ でんし けいたい たてもの きょうたい てつろ ようどう こうそくどうろ こうつうき タクシー じてんしゃ でんりょく ちず ちかてつ えき くうこう こうこう がんこう がいこう がっこう だいがく がくせい きょうしつ せんせい', 9)
INSERT [dbo].[EXERCISETEXT] ([ExerciseTextID], [ExerciseID], [Text], [LanguageID]) VALUES (17, 3, N'du lịch là một hoạt động thú vị và hấp dẫn mà mọi người đều mong muốn trải nghiệm ít nhất một lần trong đời khám phá những điểm đến mới những vùng đất lạ thường mang lại cho chúng ta những trải nghiệm tuyệt vời và những kỷ niệm đáng nhớ việc lên kế hoạch cho một chuyến du lịch đòi hỏi sự chuẩn bị kỹ lưỡng từ việc tìm hiểu về địa điểm đến đặt vé máy bay chỗ ở đến việc lên lịch trình tham quan và trải nghiệm các hoạt động vui chơi mỗi điểm đến đều mang một nét đặc trưng riêng từ những bãi biển tuyệt đẹp những ngọn núi hùng vĩ đến những thành phố sôi động và nhộn nhịp du lịch còn là cơ hội để khám phá văn hóa lịch sử và ẩm thực đặc trưng của mỗi vùng miền dù là đi du lịch một mình cùng bạn bè hoặc gia đình mỗi chuyến đi luôn để lại trong lòng chúng ta những trải nghiệm đáng nhớ và những kỷ niệm không thể nào quên', 19)
INSERT [dbo].[EXERCISETEXT] ([ExerciseTextID], [ExerciseID], [Text], [LanguageID]) VALUES (18, 3, N'in a test done in the students at the university of wyoming said what they considered attractive in their partners their answers were not new men tended to prefer blondes blue eyes and light skin color while women liked darker men however there were some surprises few men liked very large breasts or boyish women almost none of the women liked very muscular men in fact both men and women prefer the average too short too tall too pale or too dark were not chosen averageness still wins in a recent study scientists chose 94 faces of american women and used the computer to make a picture of an average face then they asked people which face they liked of the 94 real faces only  faces were considered to be more attractive than the average face most people said that they preferred the average face', 4)
INSERT [dbo].[EXERCISETEXT] ([ExerciseTextID], [ExerciseID], [Text], [LanguageID]) VALUES (19, 3, N'我 你 他 她 它 我们 你们 他们 它们 这 那 这个 那个 什么 谁 哪里 怎么 为什么 因为 如果 所以 现在 昨天 明天 今天 早上 下午 晚上 天气 天空 太阳 月亮 星星 风 雨 雪 云 雷电 彩虹 地球 大陆 海洋 山 山脚 山顶 河流 湖泊 花树 草地 花园 园林 公园 乡村 城市 街道 房屋 家庭 房间 门 窗户 地板 天花板 墙壁 书桌 椅子 床 柜子 电视 电脑 手机 建筑 桥梁 铁路 高速公路 公交车 出租车 自行车 电梯 地铁 火车站 机场 港口 学校 大学 学生 教室 老师', 2)
INSERT [dbo].[EXERCISETEXT] ([ExerciseTextID], [ExerciseID], [Text], [LanguageID]) VALUES (20, 3, N'わたし あなた 彼 彼女 それ わたしたち あなたたち 彼ら それら これ それ この その どれ だれ どこ どうして なぜ なぜなら もし ゆえに いま きのう あした きょう あさ ひる ばんてん てんくう たいよう つき ほし かぜ あめ ゆき くも かみなり にじ ちきゅう たいりく かいよう やま さんぢゅう さんけつ こうがい こうつう ぎょうとり しゃどう ふろん かくじ かしつ ゆか てんじょう かべ つくえ いす べっど つくえ でんし けいたい たてもの きょうたい てつろ ようどう こうそくどうろ こうつうき タクシー じてんしゃ でんりょく ちず ちかてつ えき くうこう こうこう がんこう がいこう がっこう だいがく がくせい きょうしつ せんせい', 9)
INSERT [dbo].[EXERCISETEXT] ([ExerciseTextID], [ExerciseID], [Text], [LanguageID]) VALUES (21, 4, N'du lịch là một hoạt động thú vị và hấp dẫn mà mọi người đều mong muốn trải nghiệm ít nhất một lần trong đời khám phá những điểm đến mới những vùng đất lạ thường mang lại cho chúng ta những trải nghiệm tuyệt vời và những kỷ niệm đáng nhớ việc lên kế hoạch cho một chuyến du lịch đòi hỏi sự chuẩn bị kỹ lưỡng từ việc tìm hiểu về địa điểm đến đặt vé máy bay chỗ ở đến việc lên lịch trình tham quan và trải nghiệm các hoạt động vui chơi mỗi điểm đến đều mang một nét đặc trưng riêng từ những bãi biển tuyệt đẹp những ngọn núi hùng vĩ đến những thành phố sôi động và nhộn nhịp du lịch còn là cơ hội để khám phá văn hóa lịch sử và ẩm thực đặc trưng của mỗi vùng miền dù là đi du lịch một mình cùng bạn bè hoặc gia đình mỗi chuyến đi luôn để lại trong lòng chúng ta những trải nghiệm đáng nhớ và những kỷ niệm không thể nào quên', 19)
INSERT [dbo].[EXERCISETEXT] ([ExerciseTextID], [ExerciseID], [Text], [LanguageID]) VALUES (22, 4, N'âm nhạc là một phần không thể thiếu trong cuộc sống hàng ngày của chúng ta nó là nguồn cảm hứng và niềm vui cho mỗi người từ những bản nhạc sôi động nhịp nhàng đến những giai điệu dịu dàng lãng mạn âm nhạc có thể thay đổi tâm trạng và tạo ra một không gian tinh thần thoải mái thư giãn từ những nghệ sĩ tài năng biểu diễn trên sân khấu đến những người yêu âm nhạc tận hưởng và chia sẻ cùng nhau âm nhạc cũng là một phần của văn hóa và truyền thống của mỗi quốc gia mỗi dân tộc từ những bài hát dân ca đến những giai điệu hiện đại âm nhạc góp phần làm phong phú thêm đời sống văn hóa và nghệ thuật không gian âm nhạc không chỉ là nơi để thưởng thức âm nhạc mà còn là nơi để kết nối và tạo ra những kỷ niệm đáng nhớ', 19)
INSERT [dbo].[EXERCISETEXT] ([ExerciseTextID], [ExerciseID], [Text], [LanguageID]) VALUES (23, 6, N'apple orange banana water milk bread butter cat dog house tree car book phone table chair sun moon star rain cloud sky flower garden river lake ocean mountain forest path road city village school university hospital church temple mosque synagogue park zoo', 4)
SET IDENTITY_INSERT [dbo].[EXERCISETEXT] OFF
GO
SET IDENTITY_INSERT [dbo].[TEXTPRACTICE] ON 

INSERT [dbo].[TEXTPRACTICE] ([ID], [UserCreate], [Text], [Title], [TextLength], [PeopleIsCompleted], [CreatedAt], [LanguageID], [JoinCode], [IsPrivate], [Rating], [ListUserLike], [ListUserRating]) VALUES (1, 1, N'Cắt đôi nỗi sầu anh buông tay cắt đôi nỗi sầu
Anh cắt đi cả bóng hình anh mang theo bên mình bấy lâu
Nỗi đau đã cạn cơn mưa trong tim cũng đã tàn
Anh bán đi mọi nỗi buồn để chẳng còn gì ngoài thanh thản

Em ơi anh muốn mỗi tối đến anh không phải thất tình
Muốn quên một bóng hình em để lại trong tim
Anh không thể đếm đã có bấy nhiêu đêm phải kiếm tìm
Kiếm thêm một lí do cho cuộc tình không tên', N'Cắt đôi nỗi sầu - Tăng Duy Tân', 92, 0, 1709558096270, 19, N'sLqkYXIvt6nBTTaaKVnH', 0, 0, N'[{"IdUser":1,"JoinCode":"sLqkYXIvt6nBTTaaKVnH","DateLike":1709701242657}]', N'[]')
INSERT [dbo].[TEXTPRACTICE] ([ID], [UserCreate], [Text], [Title], [TextLength], [PeopleIsCompleted], [CreatedAt], [LanguageID], [JoinCode], [IsPrivate], [Rating], [ListUserLike], [ListUserRating]) VALUES (2, 1, N'Tại sao luôn có những khoảng trống trong bài hát của anh khi không có một nụ cười
Và anh không biết làm cách nào để có thể vui cười trở lại, để dũng cảm nói chuyện lại với em
Vì có lẽ lúc mình xa nhau mọi thứ đã thay đổi
Và anh không biết làm cách nào để bắt nhịp với cách mà em quên anh
Khi một câu trả lời cũng không có
Vì em vẫn vậy, vẫn chỉ là những sự im lặng từ nơi em

Rồi còn muốn nói gì không em hay không thể
Giờ nên hiểu sao đây em khi mình còn gần kề
Ai sẽ nói trước, ai sẽ im lặng, ai sẽ nhường bước
Hai chúng ta phải hiểu rằng
Mọi thứ đã vượt quá xa bên ngoài tầm kiểm soát
Anh hoặc em có lẽ nên dứt khoát
Mình không nên làm tổn thương thêm nhau đâu
Không nên một lần nữa chạm vào những nỗi đau', N'Im Lặng - Lil Knight', 167, 0, 1709558595661, 19, N'zlo0T3zUEDiYj1qyo9ou', 1, 0, N'[]', N'[]')
INSERT [dbo].[TEXTPRACTICE] ([ID], [UserCreate], [Text], [Title], [TextLength], [PeopleIsCompleted], [CreatedAt], [LanguageID], [JoinCode], [IsPrivate], [Rating], [ListUserLike], [ListUserRating]) VALUES (3, 1, N'Kiếp trước có lẽ hai ta yêu nhau mà chẳng thể thành vợ chồng
Nghĩ thoáng lên mai ra sao tụi mình cùng đều hài lòng
Có thể hôm nay thương có thể tương lai buông
Có thể ta không giàu miễn ở bên nhau vui không buồn

Chớp mắt hai mươi ba mươi chiều nao rồi tụi mình cùng về già
Ai rồi sẽ phải trước sau theo một người cùng về nhà
Bước tiếp hay quên đi nghĩ lắm chi thêm suy
Ta cứ như bây giờ lo âu xa xôi để làm gì

Ta yêu là yêu vậy thôi không có khái niệm đúng và sai
Mấy đứa hay nói lời khó nghe bên nhau ta bỏ ngoài tai
We drawing over night không ai phải nghi ngờ ai
Không quan tâm bao nhiêu lần sai chỉ cần em còn thương là anh vẫn ở lại

Đừng nói đến những thứ vốn quá lớn lao
Đâu ai chắc ngày mai hai ta sẽ chẳng thể bỏ nhau
Giữ tim không hoài nghi bình yên trong ta sẽ đủ lâu
Cứ vô tư biết đâu ngày sau lại vui như tình đầu

Kiếp trước có lẽ hai ta yêu nhau mà chẳng thể thành vợ chồng
Nghĩ thoáng lên mai ra sao tụi mình cùng đều hài lòng
Có thể hôm nay thương có thể tương lai buông (có thể có thể)
Có thể ta không giàu miễn ở bên nhau vui không buồn', N'Bạn đời Bài hát của Karik', 246, 0, 1709659075049, 19, N'DvpwzCj4vblF1TOOkpwh', 0, 0, N'[]', N'[]')
INSERT [dbo].[TEXTPRACTICE] ([ID], [UserCreate], [Text], [Title], [TextLength], [PeopleIsCompleted], [CreatedAt], [LanguageID], [JoinCode], [IsPrivate], [Rating], [ListUserLike], [ListUserRating]) VALUES (4, 2, N'Liệu mai sau phai vội mau không bước bên cạnh nhau (Bên cạnh nhau)
Thì ta có đau? (Thì ta có đau? Có đau?)
Đôi mi nhoè phai ai sẽ lau?
Ai sẽ đến lau nỗi đau này?

Vô tâm quay lưng, ta thờ ơ, lạnh lùng băng giá như vậy sao? (Vậy sao? Vậy sao?)
Vờ không biết nhau (Không biết nhau, không biết nhau)
Lặng im băng qua chẳng nói một lời (Chẳng nói một lời)
Ooh-whoa-ooh-whoa-oh-oh-oh (Yeah, eh)

Rồi niềm đau có chóng quên? (Hah-ah-ooh-ah)
Hay càng quên càng nhớ thêm vấn vương, vết thương lòng xót xa?
Đừng như con nít (con nít), từng mặn nồng say đắm say (Oh-oh-ah)
Cớ sao khi chia tay (chia tay), ta xa lạ đến kì lạ? (Ta xa lạ đến kì lạ)

Ai dám nói trước sau này (Trước sau này)
Chẳng ai biết trước tương lai sau này (Sau này)
Tình yêu đâu biết mai này có vẹn nguyên
Còn nguyên như lời ta đã hứa trước đây? (Ta đã hứa trước đây)
Dẫu cho giông tố xô xa rời (Xa rời)
Còn mãi những điều đẹp đẽ say đắm một thời (Một thời)
Nụ cười và giọt nước mắt rơi từng trao cùng ta
Nhìn lại về phía mặt trời (Phía mặt trời)
(Ta về phía mặt trời, phía mặt trời, phía mặt trời) yah, yah', N'Chúng Ta Của Tương Lai "Sơn Tùng M-TP"', 229, 0, 1710003063700, 19, N'GWPB3B7ZPzhSpev93zCz', 0, 0, N'[{"IdUser":1,"JoinCode":"GWPB3B7ZPzhSpev93zCz","DateLike":1710003590919}]', N'[]')
INSERT [dbo].[TEXTPRACTICE] ([ID], [UserCreate], [Text], [Title], [TextLength], [PeopleIsCompleted], [CreatedAt], [LanguageID], [JoinCode], [IsPrivate], [Rating], [ListUserLike], [ListUserRating]) VALUES (8, 1, N'Con cá vàng
Quàng khăn lụa
Giữ nước trong
Cùng bạn múa', N'Con cá vàng', 12, 1, 1710514039724, 19, N'vTwJd2Y78GA7ALsYUZV6', 0, 5, N'[]', N'[{"IdUser":1,"JoinCode":"vTwJd2Y78GA7ALsYUZV6","Rating":5}]')
SET IDENTITY_INSERT [dbo].[TEXTPRACTICE] OFF
GO
SET IDENTITY_INSERT [dbo].[TYPINGRESULTGAME] ON 

INSERT [dbo].[TYPINGRESULTGAME] ([ResultId], [ExerciseId], [UserID], [Score], [Score2], [Timestamp], [Score3]) VALUES (1, 6, 2, 6, 11, 1710325857542, 23)
INSERT [dbo].[TYPINGRESULTGAME] ([ResultId], [ExerciseId], [UserID], [Score], [Score2], [Timestamp], [Score3]) VALUES (2, 6, 1, NULL, 14, 1710326146275, NULL)
INSERT [dbo].[TYPINGRESULTGAME] ([ResultId], [ExerciseId], [UserID], [Score], [Score2], [Timestamp], [Score3]) VALUES (3, 7, 1, 9, 4, 1710332813077, NULL)
INSERT [dbo].[TYPINGRESULTGAME] ([ResultId], [ExerciseId], [UserID], [Score], [Score2], [Timestamp], [Score3]) VALUES (4, 8, 1, 3, 11, 1710333876952, NULL)
INSERT [dbo].[TYPINGRESULTGAME] ([ResultId], [ExerciseId], [UserID], [Score], [Score2], [Timestamp], [Score3]) VALUES (5, 9, 1, 95, NULL, 1710334756254, NULL)
INSERT [dbo].[TYPINGRESULTGAME] ([ResultId], [ExerciseId], [UserID], [Score], [Score2], [Timestamp], [Score3]) VALUES (6, 9, 2, 91, 84, 1710411268670, NULL)
SET IDENTITY_INSERT [dbo].[TYPINGRESULTGAME] OFF
GO
SET IDENTITY_INSERT [dbo].[TYPINGRESULTS] ON 

INSERT [dbo].[TYPINGRESULTS] ([ResultId], [UserID], [Accuracy], [WPM], [Mistakes], [CorrectWords], [TotalWords], [Timestamp], [ExerciseTextID], [KeyStrokes], [JoinCode]) VALUES (1, 1, 100, 35, 0, 58, 58, 1708247672117, 3, 177, NULL)
INSERT [dbo].[TYPINGRESULTS] ([ResultId], [UserID], [Accuracy], [WPM], [Mistakes], [CorrectWords], [TotalWords], [Timestamp], [ExerciseTextID], [KeyStrokes], [JoinCode]) VALUES (2, 1, 95.92, 32, 2, 47, 49, 1708248132211, 10, 162, NULL)
INSERT [dbo].[TYPINGRESULTS] ([ResultId], [UserID], [Accuracy], [WPM], [Mistakes], [CorrectWords], [TotalWords], [Timestamp], [ExerciseTextID], [KeyStrokes], [JoinCode]) VALUES (3, 1, 92.45, 35, 4, 49, 53, 1708249112264, 3, 177, NULL)
INSERT [dbo].[TYPINGRESULTS] ([ResultId], [UserID], [Accuracy], [WPM], [Mistakes], [CorrectWords], [TotalWords], [Timestamp], [ExerciseTextID], [KeyStrokes], [JoinCode]) VALUES (4, 1, 100, 36, 0, 51, 51, 1708250490588, 2, 180, NULL)
INSERT [dbo].[TYPINGRESULTS] ([ResultId], [UserID], [Accuracy], [WPM], [Mistakes], [CorrectWords], [TotalWords], [Timestamp], [ExerciseTextID], [KeyStrokes], [JoinCode]) VALUES (5, 1, 92, 36, 4, 46, 50, 1708250593061, 2, 180, NULL)
INSERT [dbo].[TYPINGRESULTS] ([ResultId], [UserID], [Accuracy], [WPM], [Mistakes], [CorrectWords], [TotalWords], [Timestamp], [ExerciseTextID], [KeyStrokes], [JoinCode]) VALUES (6, 1, 83.669998168945312, 34, 8, 41, 49, 1708254189847, 9, 168, NULL)
INSERT [dbo].[TYPINGRESULTS] ([ResultId], [UserID], [Accuracy], [WPM], [Mistakes], [CorrectWords], [TotalWords], [Timestamp], [ExerciseTextID], [KeyStrokes], [JoinCode]) VALUES (7, 1, 87.720001220703125, 38, 7, 50, 57, 1708254725501, 2, 192, NULL)
INSERT [dbo].[TYPINGRESULTS] ([ResultId], [UserID], [Accuracy], [WPM], [Mistakes], [CorrectWords], [TotalWords], [Timestamp], [ExerciseTextID], [KeyStrokes], [JoinCode]) VALUES (8, 1, 90.739997863769531, 36, 5, 49, 54, 1708255229581, 3, 179, NULL)
INSERT [dbo].[TYPINGRESULTS] ([ResultId], [UserID], [Accuracy], [WPM], [Mistakes], [CorrectWords], [TotalWords], [Timestamp], [ExerciseTextID], [KeyStrokes], [JoinCode]) VALUES (9, 1, 88.889999389648438, 36, 6, 48, 54, 1708255387664, 3, 181, NULL)
INSERT [dbo].[TYPINGRESULTS] ([ResultId], [UserID], [Accuracy], [WPM], [Mistakes], [CorrectWords], [TotalWords], [Timestamp], [ExerciseTextID], [KeyStrokes], [JoinCode]) VALUES (10, 2, 95.830001831054688, 35, 2, 46, 48, 1708412759572, 2, 175, NULL)
INSERT [dbo].[TYPINGRESULTS] ([ResultId], [UserID], [Accuracy], [WPM], [Mistakes], [CorrectWords], [TotalWords], [Timestamp], [ExerciseTextID], [KeyStrokes], [JoinCode]) VALUES (11, 1, 90.739997863769531, 38, 5, 49, 54, 1708854218885, 22, 188, N'0XSKr08uNME')
INSERT [dbo].[TYPINGRESULTS] ([ResultId], [UserID], [Accuracy], [WPM], [Mistakes], [CorrectWords], [TotalWords], [Timestamp], [ExerciseTextID], [KeyStrokes], [JoinCode]) VALUES (12, 1, 92.589996337890625, 38, 4, 50, 54, 1708854812377, 22, 188, N'0XSKr08uNME')
INSERT [dbo].[TYPINGRESULTS] ([ResultId], [UserID], [Accuracy], [WPM], [Mistakes], [CorrectWords], [TotalWords], [Timestamp], [ExerciseTextID], [KeyStrokes], [JoinCode]) VALUES (13, 2, 93.0999984741211, 41, 4, 54, 58, 1708855030411, 22, 204, N'0XSKr08uNME')
INSERT [dbo].[TYPINGRESULTS] ([ResultId], [UserID], [Accuracy], [WPM], [Mistakes], [CorrectWords], [TotalWords], [Timestamp], [ExerciseTextID], [KeyStrokes], [JoinCode]) VALUES (14, 1, 100, 7, 0, 12, 12, 1709081256537, 21, 34, N'Rc1sN8NN1yF')
INSERT [dbo].[TYPINGRESULTS] ([ResultId], [UserID], [Accuracy], [WPM], [Mistakes], [CorrectWords], [TotalWords], [Timestamp], [ExerciseTextID], [KeyStrokes], [JoinCode]) VALUES (17, 1, 91.300003051757812, 33, 4, 42, 46, 1710263291033, 21, 165, N'FOLulW3Ewvd')
SET IDENTITY_INSERT [dbo].[TYPINGRESULTS] OFF
GO
SET IDENTITY_INSERT [dbo].[USER] ON 

INSERT [dbo].[USER] ([Id], [HoTen], [TaiKhoan], [MatKhau], [Email], [Avatar], [DiaChi], [DienThoai], [XacMinh], [LinkVerification], [IsEnable], [GioiThieu], [CreateDate], [TypeAccount]) VALUES (1, N'Top 1 server', N'quy', N'$2a$11$bwOJYy16WTfSr7pVqLqQsehgqLK82N34qVrHCGa0brSnKMme5ugFG', N'pingvocuc333@gmail.com', N'https://firebasestorage.googleapis.com/v0/b/login-besttyping.appspot.com/o/Avatars%2Fghostwire-tokyo.jpg?alt=media&token=d8a6eadd-d2ec-4a51-b7ae-62bbde1ead9e', NULL, NULL, NULL, N'331f5cc1-3695-4bb4-9408-915a7d3c4d54', 1, N'Gõ nhanh chỉ là nhất thời, Top 1 mới là mãi mãi :D!!', 1708179613046, NULL)
INSERT [dbo].[USER] ([Id], [HoTen], [TaiKhoan], [MatKhau], [Email], [Avatar], [DiaChi], [DienThoai], [XacMinh], [LinkVerification], [IsEnable], [GioiThieu], [CreateDate], [TypeAccount]) VALUES (2, N'Đẳng cấp top 1', N'ok123', N'$2a$11$WXhw2NgoziK/cPiD7wr5HuPCPYFTEgElvSNIXBjiVMEWVNhlIAFNS', N'pingvocuc222@gmail.com', N'https://firebasestorage.googleapis.com/v0/b/login-besttyping.appspot.com/o/Avatars%2Fuser-default.jpg?alt=media&token=a336a970-1f58-46fe-91c3-362edad77581', NULL, NULL, NULL, N'0630ec87-3fb6-4968-8107-b4514e87ff02', 1, N'Thành công luôn tìm đến người luôn nỗ lực!!!', 1708179613046, NULL)
SET IDENTITY_INSERT [dbo].[USER] OFF
GO
SET IDENTITY_INSERT [dbo].[USERPROGESS] ON 

INSERT [dbo].[USERPROGESS] ([ProgressID], [UserID], [SoTuDaGo], [SoBaiKiemTra], [CuocThiThamGia], [WPMTotNhat]) VALUES (1, 1, 773, 15, 1, 38)
INSERT [dbo].[USERPROGESS] ([ProgressID], [UserID], [SoTuDaGo], [SoBaiKiemTra], [CuocThiThamGia], [WPMTotNhat]) VALUES (2, 2, 106, 2, 0, 41)
SET IDENTITY_INSERT [dbo].[USERPROGESS] OFF
GO
USE [master]
GO
ALTER DATABASE [DbBestTyping] SET  READ_WRITE 
GO
