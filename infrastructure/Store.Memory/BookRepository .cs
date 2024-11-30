﻿using Kyrsova;
using System;
using System.Linq;

namespace Store.Memory
{
    public class BookRepository : IBookRepository
	{
		private readonly Book[] books = new Book[]
		{
            new Book(1, "ISBN 2000988813409", "Дональд Є. Кнут", "Мистецтво програмування","Перший том серії книг 'Мистецтво програмування' починається з опису основних понять і методів програмування. Потім автор зосереджується на розгляді інформаційних структур — поданні інформації всередині комп'ютера, структурні зв'язки між елементами даних і про способи ефективної роботи з ними. Для методів імітації, символьних обчислень, числових методів, методів розробки програмного забезпечення наведено приклади елементарних додатків. Порівняно з попереднім виданням, додані десятки простих, але в той же час дуже важливих алгоритмів. У відповідності з сучасними напрямками досліджень був істотно перероблений розділ математичного введення.",750m),

            new Book(2, "ISBN 9785932860458", "Мартін Фаулер", "Рефакторинг","На той час як об'єктна технологія – зокрема мова Java – стала звичайною справою, з'явилася велика кількість погано спроектованих, неефективних та малопридатних до супроводу та розширення програм. Професійні розробники програмних систем дедалі ясніше бачать, наскільки важко мати справу з такою “неоптимальною” спадщиною. Вже кілька років експерти в галузі об'єктного програмування застосовують набір прийомів, що розширюється, покликаних поліпшити структурну цілісність і продуктивність таких програм. Цей підхід, званий рефакторингом, досі залишався територією експертів, оскільки намагалися перевести професійні знання у форму, доступну всім розробникам. ",1050m),

			new Book(3, "ISBN 978-0131103627", "B. Kernighan, D. Ritchie", "C Programming Language","The authors present the complete guide to ANSI standard C programming language. Written by the developers of C, this new version helps readers keep up with the finalized ANSI standard for C while showing how to take advantage of C ' s rich set of operators, economy of expression, improved flow control, and data structures. The 2/E has been completely rewritten with additional examples and problem sets to clarify the implementation of difficult language constructs. For years, C programmers have let K&R guide them to building well-structured and efficient programs. Now this same help is available to those working with ANSI компілятори. Includes detailed coverage of the C language plus the official C language reference manual for at-a-glance help with syntax notation, declarations, ANSI changes, scope rules, and the list goes on and on.",650m),

            new Book(4, "ISBN 978-1-908435-13-2", "Джеймс Дашнер", "Той, що біжить лабіринтом","Це кінець. «БЕЗУМ» відібрав у Томаса все: життя, друзів і спогади. Але ж це нарешті позаду! Випробування майже закінчилися — лишилося останнє. Та чи переживе його бодай хтось? Однак «БЕЗУМ» не знає, що Томас пригадав значно більше, ніж можна припустити, і тому не вірить жодному слову цієї організації. Нехай правда виявиться жахливою, але Томас здолав Лабіринт, здолав випробування вогнем, і він готовий ризикнути всім, щоб урятувати друзів. Прийшов час покласти край брехні й облуді.",288m),

            new Book(5, "ISBN 978-1-906437-79-5", "Джеймс Дашнер", "Випробування вогнем","Втеча з лабіринту мала відкрити Томасу й решті глейдерів шлях на свободу. Тільки виявилося, що тести ще не закінчилися, і тепер їх чекає друга фаза — випробування вогнем у розжареній пустелі, яку дуже влучно назвали Пеклом. На те, щоб перетнути Пекло й дістатися безпечного прихистку, глейдери мають два тижні. Але тепер у них є конкуренти, тож ставки підвищуються. Кому вдасться дійти до кінця й не загинути?",300m),

            new Book(6, "ISBN 978-1-908435-20-0", "Джеймс Дашне", "Ліки від смерті","Це кінець. «БЕЗУМ» відібрав у Томаса все: життя, друзів і спогади. Але ж це нарешті позаду! Випробування майже закінчилися — лишилося останнє. Та чи переживе його бодай хтось? Однак «БЕЗУМ» не знає, що Томас пригадав значно більше, ніж можна припустити, і тому не вірить жодному слову цієї організації. Нехай правда виявиться жахливою, але Томас здолав Лабіринт, здолав випробування вогнем, і він готовий ризикнути всім, щоб урятувати друзів. Прийшов час покласти край брехні й облуді.",300m),

            new Book(7, "ISBN 978-617-15-1170-5", "Стівен Кінг", "Команда скелетів","«Команда скелетів» - це неймовірна збірка моторошних історій, які занурять вас в атмосферу хаосу і безвиході та добряче полоскочуть нерви.<p>" + "<p>Що коли звичайне горище перетвориться на справжнє пекло, а буденна подорож автомобілем - закінчиться в геть неочікуваному пункті призначення? Чи може бути щось більш лячне, ніж місця описаних подій? Хіба що самі герої оповідань. Бабуся, яка прагнула понад усе обійняти свого онука… навіть після смерті, чи невинна іграшкова мавпочка, що таїть у собі суще зло, чи інші персонажі, які жахатимуть своїми намірами…",490m),

            new Book(8, "ISBN 978-617-15-1172-9", "С. Солтані", "Перетнувши межу","<p>Миттєвий Бестселер USA Today<p>" + "<p>Через скандал у соцмережах карʼєра гонщика Дева Андерсона під загрозою. Хлопцю потрібен хтось, хто врятує його репутацію. Зірка Формули-1 навіть знає, до кого звернутись, але є один нюанс… <p>" + "<p>Людина, яка може реабілітувати репутацію Андерсона, — Віллов Вільямс, молодша сестра його найкращого друга, яку він необачно поцілував минулого року і досі не може про це забути. <p>" + "<p> Ця дівчина в усьому знаходить позитивні моменти і здатна виправити будь-яку ситуацію. Однак гонщик ще не знає, що Віллов з дитинства закохана в нього, тож сама не в захваті від їхньої співпраці.<p>" + "<p>Попри свої почуття, Дев та Віллов мають намір будувати лише професійні стосунки. Та чи зможуть вони дотриматись обіцянки, якщо обоє так і хочуть перетнути цю межу?..",320m),

            new Book(9,"ISBN 978-617-15-1170-5","Ребекка Яррос","Останній лист","<p>«Останній лист» - це історія кохання, яка захоплює, розбиває серце і, зрештою, надихає.<p>" +"<p>Бекетте!<p>" +"<p>Якщо ти читаєш це, то знаєш, що таке остання воля. Ти вижив. А я ні. Я знаю, що якби був шанс врятувати мене, ти б це зробив. " +"<p>Мені потрібно від тебе одне: звільнися з армії, щоб приїхати в Телльюрайд." +"<p>Моя молодша сестра Елла сама виховує близнюків. Вона надто незалежна і не хоче приймати допомогу, але вона втратила нашу бабусю,батьків, а тепер і мене. Це занадто для будь-кого. Це несправедливо.<p>" +"<p>Є ще дещо, що розриває її сім’ю на частини. Їй потрібна допомога.<p>" +"<p>Я не зможу бути поруч із Еллою. Але ти можеш. Тому я благаю тебе як свого найкращого друга: подбай про мою сестру, про мою сім’ю.<p>" +"<p>Будь ласка, не змушуй її проходити через це наодинці.<p>" +"<p>Раян.<p>" +"<p>Завдяки прекрасному, захопливому письму Яррос читачі в цьому пронизливому любовному романі відчують кожен глибокий сердечний біль і кожну мить піднесеного кохання.",320m),

            new Book(10, "ISBN 978-617-15-0882-8", "Ребекка Яррос", "Незавершені справи","<p> Після складного розлучення Джорджія втратила майже все - будинок, друзів, власну гордість і віру в кохання. Повернувшись у маєток покійної прабабусі, відомої письменниці, вона опиняється віч-на-віч із Ноа Гаррісоном, автором бестселерів про кохання. Саме він має дописати останній роман її прабабусі. Та кандидатура цього зарозумілого улюбленця долі зовсім не влаштовує Джорджію! <p>" +"<p> Ноа теж не в захваті. Придумати гідний фінал для роману легендарної авторки - це одне, а ось мати справу з її вродливою, впертою, цинічною правнучкою - зовсім інше. <p>" +"Повітря між ними іскрить від напруги! Однак листи прабабусі змушують Джорджію інакше поглянути на Ноа...",320m),

            new Book(11, "ISBN 978-617-15-1167-5", "Аґата Крісті", "Різдво Еркюля Пуаро","<p> Симеон Лі, деспотичний багатій, наполягає, щоб четверо його синів разом з родинами приїхали додому на Святвечір. Різдво - лише привід, щоб зібрати всіх в одному будинку, адже голова родини має на меті зовсім не затишне святкування. Напруга в домі зростає, коли Симеон оголошує про наміри позбавити синів фінансової підтримки та змінити заповіт. <p>" + " Невдовзі господаря маєтку знаходять у його кабінеті з перерізаним горлом. Еркюль Пуаро, який випадково перебував у сусідньому селі, погоджується допомогти в розслідуванні. У будинку панує атмосфера взаємної підозри, а кожен член родини має власні мотиви для злочину. ",230m),

            new Book(12, "ISBN 978-617-15-1149-1", "Аґата Крісті", "Немезида","<p> Міс Марпл одержує листа від свого друга Джейсона Рейфаєла з його останньою волею. У ньому той просить узятися за розслідування таємничої справи. Однак не каже якої: немає навіть натяку не те, що й коли сталося! <p>" + "<p> Невдовзі леді-детектив отримує першу підказку - поїздку, яку Джейсон організував для неї незадовго до смерті. Проте в цій подорожі починають траплятися дивні речі. Під час відвідин одного з визначних місць сталася жахлива трагедія: зі скелі зірвався камінь, унаслідок чого загинула екскурсантка. <p>" + "Що ж це було - нещасний випадок чи вбивство? Здається, хтось твердо вирішив завадити міс Марпл розкрити загадкову справу… ", 230m),

            new Book(13, "ISBN 978-617-15-0096-9", "Аґата Крісті", "Тріснуло дзеркало","<p> Міс Марпл розслідуватиме злочин за злочином! Урочистий прийом у відомої акторки Марини Ґреґ вилився у трагедію: палку шанувальницю зірки вбила отрута. Смертоносним виявився коктейль, яким сама Марина поступилася жінці замість пролитого! Якщо мішенню була Ґреґ, то чому не вона стала наступною жертвою? <p>" +"Аґату Крісті знають у всьому світі як королеву детективу. Продано близько мільярда примірників її творів англійською мовою, ще один мільярд — у перекладі 100 іноземними мовами. Вона — найпопулярніший автор усіх часів. За кількістю перевидань її книжки поступаються лише Біблії та творам Шекспіра.", 230m),

            new Book(14, "ISBN 978-617-15-0788-3", "Метью Блейк", "Анна О","<p> Спляча красуня чи спляча вбивця? <p>" + "<p> Анна Огілві - молода двадцятип’ятирічна письменниця, на яку чекає яскраве майбутнє. Під час сімейного відпочинку в одному з будиночків знаходять убитими двох її найкращих друзів, а поруч - сплячу Анну. Усі докази вказують на те, що вбивця саме вона, проте її вину неможливо довести або спростувати, адже від дня трагедії минуло чотири роки, а дівчина так і не прокинулась. <p>" +"Доктор Бенедикт Прінс - судовий психолог, який спеціалізується на злочинах, пов’язаних зі сном. Спляча красуня, як відтепер називають Анну, - загадка для багатьох фахівців. Проте саме методи роботи доктора Прінса - це остання надія розбудити можливу вбивцю, щоб вона постала перед судом і дала відповіді, які знімуть завісу з того страшного дня.",320m),

            new Book(15, "ISBN 978-617-15-0783-8", "Джесса Гастінґс", "Маґнолія Паркс. Книга 1","<p> Вона - вродлива, заможна, егоцентрична та злегка невротична лондонська світська левиця. Він найчастіше фотографований у Лондоні поганець, який розбив їй серце. <p>" + "<p> Маґнолія Паркс та Бі Джей Баллентайн - створені одне для одного, - цього жоден не заперечуватиме. <p>" +"<p> Вона зустрічається з іншими, щоб тримати його на відстані; він спить з іншими дівчатами, щоб відплатити їй. Однак після кожної невдалої спроби розлюбити вони все одно приповзають назад одне до одного. Щоправда, з кожним разом їхня дисфункція настигає, розриває по швах, руйнуючи світ, що вони збудували. Світ, у якому жоден із них так і не зміг відпустити іншого. <p>" +"Та коли зрештою починають проявлятися тріщини, і секрети випливають на поверхню, Маґнолії та Бі Джею не залишається ніщо іншого, окрім як подивитися у вічі непереборному питанню, що вони уникали чи не все життя, - скільки кохань тобі випадає за життя?",400m),

            new Book(16, "ISBN 978-617-15-1110-1", "Джесса Гастінґс", "Дейзі Гейтс. Книга 2","<p> Усе, чого хоче двадцятирічна Дейзі Гейтс, - це нормальне життя, однак таке неможливо. Дванадцять років тому батьків убили в неї на очах, тож опікуватися дівчинкою став старший брат Джуліан. У спадок їм перейшов злочинний бізнес родини й усі пов’язані з цим ризики. І так непросте життя Дейзі стає зовсім нестерпним, коли вона закохується в Крістіана Геммеса - одного з небагатьох людей, які не піддаються впливу її брата. <p>" + "<p>Крістіан радо грає в кохання й за стосунками з Дейзі приховує свої справжні почуття. У його серці є місце лише для Маґнолії, дівчини найкращого друга. <p>" + "Неочікувано роман Дейзі й Крістіна переростає в дещо більше, ніж проста інтрижка. Однак у цьому світі за брехню, зраду, кохання доводиться платити…",400m),

            new Book(17, "ISBN 978-617-15-1186-6", "Дафна дю Мор’є", "Ребекка","Вона вийшла заміж за багатого англійського аристократа Максиміліана де Вінтера. Але тінь його колишньої дружини нависла над її життям. Нова місіс де Вінтер намагається догодити чоловікові, стати справжньою аристократкою та бути не менш чудовою, ніж Ребекка — її попередниця, яку, здавалося, усі любили. Ходять чутки, що вона трагічно загинула: потонула, вийшовши на яхті в море вночі під час негоди. Усе змінить бал, адже місіс де Вінтер за порадою економки вдягне костюм, у який колись була вбрана й Ребекка. Ця фатальна помилка розкриє таємницю смерті колишньої дружини Максиміліана… І докорінно змінить життя її наступниці.",320m),

            new Book(18, "ISBN 978-617-15-0898-9", "Джоджо Мойєс", "Останній лист від твого коханого","<p>У пошуках цікавого матеріалу, молода журналістка, на ім’я Еллі натикається в архіві на старі листи, написані палким закоханим чоловіком. Слова в листі настільки щирі та зворушливі, що вона вирішує відшукати ким вони були написані та кому призначалися. Але Еллі не могла й подумати, що поки буде шукати чуже кохання, знайде своє. <p>" +"А за півстоліття до цього молода аристократка Дженніфер потрапляє в автокатастрофу, яка забрала у неї пам’ять. Вона погано пам’ятає своє життя до аварії та відчайдушно намагається дізнатися, хто надсилав їй любовні листи, підписавшись загадковою літерою «Б». Чи вдасться їй дізнатися правду та повернути втрачені спогади?.. Так книжка «Останній лист від твого коханого» стала історією двох жінок, які шукали любов...",280m),

            new Book(19, "ISBN 978-617-15-0514-8", "Фенні Флеґґ", "Вітаємо в цьому світі, Крихітко! Книга 1","<p>У невеличкому затишному містечку Елмвуд-Спринґз жила маленька чарівна дівчинка Дена, яку ніжно прозвали Крихіткою. Але одного дня їй разом з мамою довелось терміново поїхати з міста - про причини їхньої втечі так ніхто й не дізнався.\r\n <p>" +"Тридцять років по тому красуня Дена будує блискучу кар’єру на телебаченні. Неймовірний успіх та, здавалось, омріяне життя майже у неї в руках… але трагічні таємниці минулого не дають спокою Крихітці.",320m),

            new Book(20, "ISBN 978-617-15-1114-9", "Фенні Флеґґ", "Верхи на веселці. Книга 2","<p>Елмвуд-Спринґз - містечко, де всі всіх знають і де вирує життя - щасливе й трагічне, стабільне й непередбачуване… Тут кожен мешканець має свою родзинку, а зазвичай і не одну. <p>" + "<p>Харизматичний Гемм Спаркс, занадто амбітний, щоб займатися продажем тракторів, балотується в губернатори, а потім у президенти. А дві жінки, які його кохають, із подивом спостерігають, що з того вийде. <p>" +"Беатріс Вудс, «маленька сліпа співоча пташка», погоджується на неочікувану пропозицію. Життєрадісна тітка Елнер, як і годиться літній жінці, бере під опіку ціле сімейство рудих котів. А Сусідка Дороті у щоденних радіопередачах повідомляє про них та інших мешканців містечка на весь світ! Бо кожному з Елмвуд-Спринґза є що вам розказати, чим підтримати в похмурий день і як надихнути радіти кожній миті життя!",320m),

            new Book(21, "ISBN 978-617-15-1148-4", "Фенні Флеґґ", "Заберіть мене на небеса. Книга 3","<p>Тепла та життєствердна історія про жителів маленького містечка <p>"+"<p>Життя - дивовижна річ. Щойно Елнер Шимфессл сиділа на дереві, збираючи інжир, а вже наступної миті знепритомніла і... вирушила у пригоду, про яку навіть не мріяла! <p>"+"<p>Тим часом її нервова племінниця Норма опиняється в ліжку з холодним компресом на голові. Сусідка Елнер негайно кидається до Біблії. А її друг Лютер, водій вантажівки, з’їжджає в кювет... <p>"+"І все місто завмирає у німому запитанні: для чого взагалі існує життя?",320m),

            new Book(22, "ISBN 978-617-15-1155-2", "Люко Дашвар", "Мати все","<p>Молодість професорської доньки Лідочки Вербицької здається безхмарною: турботлива мама й коханий чоловік, гарна квартира, перспективна робота. Але родина дівчини має чимало темних таємниць, і Лідин брат, що ніколи не виходить з кімнати, - не єдина маріонетка в руках матері! <p>"+"<p>Підступ і брехня - ось на чому мати побудувала родинну ідилію! <p>"+"Власного щастя Ліда може зректися сама. Адже мати все - і все втратити, виявляється, не так і складно. Особливо якщо твоя мати - твоє все…",200m),

            new Book(23, "ISBN 978-617-15-1161-3", "Світлана Талан", "Матусин оберіг","Олесі було лише вісім, коли вона втратила маму. Перед смертю та залишила дочці теку з листами. Дівчинка має відкривати їх по одному щороку на день народження. Ці листи стають її єдиною опорою в численних життєвих випробуваннях, кожне з яких могло б зламати долю… Подорослішавши, дівчина знаходить справжнє кохання. Та настає тривожний 2014 рік, тож Олеся не зі своєї вини змушена відступитися від мрії. Лист мами спонукає її виправити все. Та чи зможе материнська любов уберегти від пекла війни?",200m),

            new Book(24, "ISBN 978-617-15-1135-4", "Чак Поланік", "Бійцівський клуб","<p>Бій триватиме стільки, скільки потрібно <p>"+"<p>Якщо можна постійно прокидатися в різних місцях і в різні часи, чому б одного разу не прокинутися іншою людиною? Саме такої істини доходить певного дня безіменний герой цієї історії. <p>"+"<p>Досягти бажаного, внести новий зміст у переповнену безсонням реальність йому допоможе бунтівник на ім’я Тайлер Дьорден. Той приводить героя у місце, що зветься бійцівським клубом. І перше правило клубу - ніколи нікому не говорити про нього. <p>"+"<p>Тут збираються ті, хто скніє в полоні буденності, хто, б’ючись, хоче знову відчути себе чоловіком. Ті, хто обирає: пекло або ніщо. Чоловіки, які за межею болю, поза рамками етики, на краю божевілля хочуть здобути дику, первісну свободу. Але першим кроком до цієї свободи має стати не бій, а смерть. <p>"+"Чака Поланіка називають \"шоковим письменником\". Нащадок українського емігранта, він розпочав літературну кар’єру в досить зрілому віці, уже маючи, що сказати читачам. Його книжки давно сягнули за межі власне художнього твору і стали явищем контркультури. Стиль Поланіка - різкий, категоричний, провокативний - це маніфест покоління Х. Твори письменника здобули нагороди Pacific Northwest Booksellers Association Award і Oregon Book Award, а також кілька разів номінувалися на Премію Брема Стокера.",270m),

            new Book(25, "ISBN 978-617-15-1136-1", "Оксана Кір’ян", "Барвінок хрещатий","<p>Колись Тетяна мріяла стати лікаркою, жити з коханим та ростити діточок. Але доля склалася інакше. Проте Тетяна не впала у відчай і, слідуючи давній меті, влаштувалася працювати медсестрою у рідному селі, щоб хоч так бути ближче до бажаної професії. Наче життя налагоджується, тільки жіноче щастя її оминає... <p>"+"Несподівано в селі з’являється новий лікар - Денис. Тетяна відчуває, що її тягне до нього, та чоловік значно молодший за неї… Чи дозволить вона собі таке омріяне кохання?",200m),

            new Book(26, "ISBN 978-617-15-1133-0", "Марк Леві", "Між небом і землею","<p>Лорен жила звичайним життям. У буденних радощах та дрібних проблемах. Працювала в лікарні, допомагала людям. Доки одного разу не потрапила в жахливу аварію. Вона опиняється в комі, за крок від смерті та життя. <p>"+"<p>Артур — молодий архітектор. Він винаймає квартиру, планує щоденні справи. Одного дня хлопець зустрічає в помешканні молоду дівчину. Виявляється, вона — власниця квартири. Її звуть Лорен, і вона не розуміє, чому саме Артур бачить її... <p>"+"А якщо Артур — остання нитка, яка не дає Лорен втратити себе десь між небом і землею? І лише його любов здатна повернути її до життя...",270m),

            new Book(27, "ISBN 978-617-15-1132-3", "Марк Леві", "Така, як ти","Будинок № 12 на П’ятій авеню у Нью-Йорку. Звичайна будівля, у якій немає нічого особливого. Хіба що старовинний механічний ліфт. Та загадковий ліфтер-індус Санджай. Хлоя, власниця квартири з дев’ятого поверху, і не здогадується, ким насправді є новий ліфтер. Як не знає й про те, що він готовий віддати всі скарби світу, аби знову опинитися на дев’ятому поверсі. Відчинити двері, зазирнути їй в очі й промовити: «Я все життя шукав таку, як ти». Між ними — вісім поверхів, одна маленька таємниця й бурхливе, сповнене несподіванок життя...",270m),

            new Book(28, "ISBN 978-617-15-1165-1", "С. Талан", "Просто гра","<p>Надворі кінець 90-х. Ілля — простий автослюсар. Що він міг дати доньці заможного підприємця? Кохання? Звучить романтично, але без фінансового підґрунтя ця казка швидко скінчиться. <p>"+"<p>Щоб розпочати свою справу, Ілля наважується попросити грошей у кримінального авторитета на прізвисько Латиш. Але «ділова угода» завершується кримінальними розборками. До гри несподівано долучається Марго, дружина Латиша. Вона рятує хлопця, тікаючи разом із ним. <p>"+"Шляху назад немає. Там лишилося все, чим жив Ілля: його кохана та їхня ненароджена дитина. Тепер він — утікач. Але Ілля не сам. Він відчуває, як без бою віддає своє серце Марго, закохуючись у цю небезпечно привабливу жінку. Ілля гадає, що Марго врятувала його. Але навіщо — знає лише вона...",200m),

            new Book(29, "ISBN 978-617-15-0868-2", "Світлана Талан", "Ми завжди були разом","<p>Римма завжди боялася змін. Але коли в неї діагностували рак, жінка зрозуміла, що настав час бути собою і жити так, як хочеться. <p>"+"Випадково Римма натрапляє на дивне оголошення в газеті: чоловік на ім’я Влад шукає досвідчену даму, яка навчить його розуміти, чого хочуть дівчата та як із ними спілкуватися. Риммі уже нема чого втрачати, і вона погоджується на зустріч, водночас навіть не здогадуючись, як звичайне знайомство здатне змінити її життя…",200m),

            new Book(30, "ISBN 978-617-15-0644-2", "О. Калина", "Батерфляй","<p>Що приховує наш страх? <p>"+"<p>Віта має життя, про яке мріє більшість жінок. У свої 42 роки вона привертає увагу молодих та зрілих чоловіків, має престижну високооплачувану роботу й, незважаючи на розлучення, теплі взаємини зі своїми донечками. <p>"+"<p>Єдине, що заважає жінці повноцінно насолоджуватись життям, - це її дивна фобія. З невідомої для себе причини Віта жахливо боїться метеликів, але ігнорує цей страх. Проте все ж доведеться звернути на нього увагу, адже в її житті раптово з’являється загадковий чоловік із брошкою у формі метелика. <p>"+"Чи зможе Віта перемогти страх і наново відбудувати своє життя, яке зруйнувала зустріч із цим таємничим чоловіком?..",200m),

            new Book(31, "ISBN 978-617-15-0053-2", "Дарина Гнатко", "Щоденник безнадійно приреченої","Євгенія, Женя, Єуженія… Її по-різному називали упродовж останніх трьох років Другої світової війни, поки вона долала важкий шлях, який довелося пройти від рідного українського села до Польщі, з Польщі до Німеччини й назад. Євгенія Скриль вижила під час Голодомору, її сім’я зазнала утисків з боку комуністів, пізніше у пам’яті закарбувалась жорстокість німецьких нацистів. Козацьке коріння, прагнення волі й доброта людей дали Євгенії змогу вибратися живою з двох таборів смерті, а також пізнати справжнє кохання серед страждань і зради. Однак чи судилося їй разом з коханим чоловіком подолати всі перепони? Про це розкажуть сторінки щоденника.",200m),

            new Book(32, "ISBN 978-617-17-0473-2", "Е. Діас", "Довіра","<p>Бенджамін і Гелен Раски немов створені одне для одного. Ексцентричні напіввідлюдники, що мають статус небожителів і небачені статки. Він — орудар на фінансовому ринку, вона займається доброчинністю. <p>"+"Аж раптом у Гелен зроджується тривожність, проростаючи безладними монологами. Чи Бенджамін слухатиме? Чи зможе витягти дружину з мороку божевілля? Про смерть героїні з перших сторінок роману «Зобов’язання» такого собі Гарольда Веннера свого часу дізнався мало не весь Нью-Йорк.",450m),

            new Book(33, "ISBN 978-617-12-6049-8", "Андрій Кокотюха", "Багряний рейд","Максим Коломієць, колишній радянський міліціонер, потім — в’язень ГУЛАГу та диверсант, волею долі потрапляє у волинські ліси та приєднується до українських повстанців. У цьому романі Коломієць — уже командир відділу УПА на псевдо Східняк. А східнякам в УПА не дуже довіряють. Проте ніхто, крім Коломійця, не може очолити бойову групу, яка вирушає в небезпечний рейд з Волині на Київщину. Тут, у радянському тилу, повстанці мають вести агітацію й готувати диверсії. Не всі дійдуть до місця живими. Не всі виживуть у сутичці з загонами НКВС. А ті, хто вижив, помстяться за смерть побратимів.",180m),

            new Book(34, "ISBN 978-617-8286-17-0", "Меґ Вейт Клейтон", "Останній потяг до Лондона","<p>«Останній потяг до Лондона» — заснована на реальних подіях, книга про кохання, втрату та героїзм, що стала національним бестселером у США, Канаді та Європі, фіналістом Національної єврейської книжкової премії та була перекладена 19 мовами. <p>"+"<p>Історія в романі розгортається 1936 року в Австрії. Штефан Нойман — п’ятнадцятирічний син заможної та впливової єврейської сім’ї та починаючий драматург, живе у величезному будинку з батьками та молодшим братом. Зофі-Гелена, його найкраща подруга, талановита дівчинка, мати якої редагує прогресивну антинацистську газету. Підлітки спілкуються про театр і літературу, досліджують потаємні віденські підземелля, діляться творчими й науковими ідеями та мріють про великі звершення. Проте навіть не здогадуються, що у березні 1938 року, після аншлюсу Австрії нацистською Німеччиною, їхні життя кардинально зміняться. <p>"+"<p>Однак у темряві є надія. Трюйс Вейсмюллер-Меєр, учасниця голландського руху опору, ризикує своїм життям, організовуючи вивезення єврейських дітей із окупованих нацистами територій до безпечних країн. Її місія стає ще небезпечнішою, оскільки по всій Європі країни закривають свої кордони для зростаючої кількості біженців, які відчайдушно прагнуть втекти. Чи зможе Трюйс врятувати Стефана та Зофі та захистити їх у небезпечній подорожі в невизначене майбутнє за кордон?<p>"+"«Останній потяг до Лондона» — художній роман, написаний на основі відомостей про справжню програму «Кіндертранспорт», яку очолювала Гертруда Вейсмюллер-Меєр, активістка голландського руху опору. За час своєї діяльності жінка змогла вивезти близько десяти тисяч єврейських дітей. Діти називали її «тітонька Трюйс».",435m),

            new Book(35, "ISBN 978-617-15-1123-1", "Майкл Роботам", "Хороша дівчинка, погана дівчинка. Книга 1","<p>Іві Кормак - дівчина без минулого. Шість років тому її знайшли в секретній кімнаті після скоєння жахливого злочину. Зараз дівчина вимагає права покинути дитячий будинок як повнолітня, і судовий психолог Сайрус Гевен має визначити, чи готова вона до цього. Іві володіє унікальним даром: вона точно знає, коли хтось бреше, але не вміє себе контролювати. Це робить її небезпечною для себе й оточення. <p>"+"<p>Джоді Шиген - ідеальна дівчина, точніше, була нею до трагічної загибелі. Сайрус починає розслідування її жорстокого вбивства й відкриває приголомшливі подробиці таємного життя Джоді. <p>"+"Справи двох дівчат переплітаються, затягуючи Сайруса у світ брудних секретів, де ніхто не говорить правди і лише одна людина знає, хто бреше.",320m),

            new Book(36, "ISBN 978-617-15-0095-2", "Аґата Крісті", "Оголошено вбивство","<p>Хто вбив… убивцю? <p>"+"<p>Оголошення в щоденній газеті неабияк заінтригувало мешканців англійської провінції: їх запрошено на дивну виставу — вбивство. Охочі подивитися на це, а їх виявилося чимало, певно, забули, що весь світ — театр, а люди в ньому — актори, а не глядачі. Тож ролі жертв уже розподілено… <p>"+"Тільки старенькій міс Марпл до снаги викрити загадкового постановника кривавої драми.",230m),

            new Book(37, "ISBN 978-617-15-1113-2", "Аґата Крісті", "Убивство в маєтку Голлов","<p>Еркюля Пуаро запрошено на званий обід у заміський маєток заможної родини. Господиня - доволі ексцентрична леді Енкейтелл, тож відомий детектив навіть не сумнівається, що сцена, яку він застав біля басейну, розіграна спеціально для нього, до того ж вельми поганенько… <p>"+"<p>Однак одразу з’ясовується, що це не гра невмілих акторів: кров справжня, чоловік дійсно помирає, а його дружина тримає аж ніяк не іграшковий револьвер. Перед смертю жертва встигає вимовити ім’я, проте чи все так просто у цій справі? <p>"+"Що більше Пуаро дізнається про гостей родини Енкейтеллів, то сильніше заплутується в павутині їхніх химерних взаємин. Здається, кожен з них може бути підозрюваним, адже кожен є жертвою складних любовних інтриг…",230m),

            new Book(38, "ISBN 978-617-15-1112-5", "Аґата Крісті", "Годинники","<p>Стенографістка Шейла Веб приходить на зустріч до поважної сліпої пані. Проте у вітальні її зустрічає не господиня - натомість вона натрапляє на мертве тіло… <p>"+"<p>Ніхто не знає, ким був цей чоловік і звідки він узявся. Навіщо вбивця розклав довкола жертви кілька годинників і що означає час на них? Колін Лемб, який волею долі виявився залученим до розслідування, не знаходить відповідей. Тому звертається по допомогу до Еркюля Пуаро. <p>"+"Знаменитий детектив давно відійшов від справ, але завжди радий задіяти свої сірі клітини. «Цей злочин настільки складний, що має бути досить простим», - заявляє він. Однак убивця розгулює на волі, а час спливає…",230m),
            
            new Book(39, "ISBN 978-617-15-0785-2", "Аґата Крісті", "Третя дівчина","<p>Норма Рестарік переконана, що вбила людину. Проте вона не може пригадати ані імені жертви, ані знаряддя вбивства, ані місце злочину. Відчуваючи докори сумління, Норма звертається по допомогу до видатного бельгійського детектива Еркюля Пуаро. <p>"+"Спершу Пуаро здається, що жінка душевнохвора, а її родині на це байдуже. Та що глибше детектив поринає в деталі справи, то більше переконується: лондонська богема - зовсім не така, якою хоче видаватися…",230m),

            new Book(40, "ISBN 978-617-15-0063-1", "Аґата Крісті", "Таємниця відірваної пряжки","Знаменитий детектив Еркюль Пуаро має одну маленьку слабкість: він терпіти не може візити до дантиста. А тут ще й довелося відвідати кабінет доктора Морлі двічі поспіль. Щоправда, вдруге Пуаро потрапляє вже не на прийом, а на місце злочину. Пістолет у руці померлого вказує на самогубство, але, коли останні клієнти дантиста помирають або зникають за дивних обставин, сумнівів не лишається: це справа рук таємничого зловмисника. Що більше відповідей дізнається детектив Пуаро, то більше запитань у нього з’являється. І кожне зловісніше за попереднє…",230m),

            new Book(41, "ISBN 978-617-12-9152-2", "Стівен Кінг", "Згодом","Джеймі — в усьому звичайний хлопчик зі звичайної неповної сім’ї, крім одного. Він бачить мертвих і може розмовляти з ними. До того ж покійники мусять завжди чесно відповідати на його запитання. Мама наказувала нікому не розповідати про цю моторошну здібність, проте зберегти таємницю не вдалося. Відчайдушна детективка, мамина подруга, планує розкрити справу зловісного нью-йоркського терориста з допомогою хлопчика. Убивця вже відійшов у засвіти, а його бомба цокає десь у місті й ось-ось збере кривавий урожай. Джеймі може дізнатися, де він її заховав. Але цей мрець не схожий на інших...",270m),

            new Book(42, "ISBN 9786171510043", "Стівен Кінг", "Голлі","<p>Найхаризматичніша героїня Стівена Кінга повертається! <p>"+"<p>Детектив Голлі Гібні мріє про відпустку: на нову справу в неї просто немає сил. Однак почувши відчайдушну мольбу в голосі Пенні Дал, Голлі без вагань погоджується допомогти жінці в пошуках доньки. <p>"+"Недалеко від місця, де зникла Бонні Дал, живе немолода сімейна пара професорів — Родні та Емілі Гарріси. Шановані, респектабельні, віддані одне одному… а ще підступні й безжальні. Підвал їхнього охайного, заставленого книжками будинку приховує жахливу таємницю. Якщо Голлі припуститься помилки, то познайомиться із цим моторошним місцем дуже близько...",260m),

            new Book(43, "ISBN 978-617-12-5064-2", "Стівен Кінг", "Аутсайдер","<p>Тренер молодіжної бейсбольної команди, викладач англійської, чоловік та батько двох доньок. Усе це про Террі. Так, про таких кажуть «класний чувак», з таким усі хочуть дружити і такому не бояться позичити грошей. Так, Террі крутий. А ще — убивця. Це ж він вчинив ту моторошну наругу над нещасним одинадцятирічним хлопчиком? Так, він. Хто б міг подумати, Террі, хто б міг подумати... Поліція має усі докази. А Террі — залізобетонне алібі: на момент убивства він перебував у іншому місті. Та як людина може бути у двох місцях одночасно? <p>"+"Що відбувається в містечку? Тут живе дещо жахливе. Те, що, напевно, може набувати людської подоби. Чи те, що нарешті скинуло з себе маску людини та почало свої криваві жнива...",420m),

            new Book(44, "ISBN 978-617-12-9797-5", "Стівен Кінг", "Довга Хода","Тоталітарне майбутнє. У щорічному змаганні для хлопців під назвою «Довга Хода» переможець лише один і він отримає все. На решту учасників чекає смерть. Шістнадцятирічний Рей наважується взяти участь у «Довгій Ході». Його суперники — 99 підлітків. Кожен з них прагне перемогти і вижити. Для цього потрібно весь час іти заздалегідь визначеним маршрутом зі швидкістю не менше ніж чотири милі на годину. Кожен непослух карається попередженням.За годину їх можна отримати не більше трьох. Замість четвертого — розстріл. Рей і 99 хлопців починають змагатися зі смертю. Але до кінця дійде лише один.",330m),

            new Book(45, "ISBN 978-617-12-9330-4", "Стівен Кінг", "Воно","Колись давно семеро підлітків лицем до лиця зіткнулися із невимовним Жахом — і змогли перемогти. Але багато років по тому істота, що не має імені, повертається, щоб помститися... Воно наче випірнуло з нічних кошмарів. Воно живиться страхом і ненавистю. Воно причаїлося всюди... Старі друзі мусять зустрітися з Ним і знову зазирнути у вічі справжньому жаху...",570m),

            new Book(46, "ISBN 978-617-12-8444-9", "Стівен Кінг", "Мертва зона","Коли через майже п’ять років Джон Сміт вийшов з коми, він, певно, пожалкував про це. За цей час тіло стало майже чужим, його дівчина обрала іншого, а рідна мати втратила зв’язок з реальністю. А ще треба вчитися жити з аномальною здібністю — бачити чуже майбутнє, лише торкнувшись людини. Тож повернутися до нормального життя не вийде. Особливо після того, як на передвиборчому мітингу Ґреґа Стіллсона Джонні тисне кандидатові руку. Миттєве передбачення вражає: Стіллсон виграє президентську гонку, очолить країну, а потім розв’яже ядерну війну та спричинить глобальну катастрофу. Проте змінити хід історії ще можливо. І в цьому заплутаному лабіринті Джонні бачить лише один вихід: вбити Ґреґа Стіллсона.",360m),

            new Book(47, "ISBN 978-617-12-8930-7", "Стівен Кінг", "Та, що породжує вогонь (Палійка)","Це почалося, коли юні Енді та Вікі, шукаючи підзаробіток, взяли участь у тестуванні таємничого препарату. Після експерименту вони відкрили в собі екстрасенсорні надздібності. Згодом у пари народилася дівчинка Чарлі. У дворічному віці вона спопелила іграшкового ведмедика самим лише поглядом. Донька отримала від батьків новий, особливий дар — пірокінез. Тепер Чарлі загрожує небезпека. Вона — дамоклів меч, і не лише для оточення, а й для власної родини. Урядова організація, що колись провела зловісний експеримент за участі батьків Чарлі, хоче заволодіти дитиною з такими суперздібностями. І зовсім не з метою допомогти. Чарлі — ідеальна зброя. А зброя має виконати своє вбивче призначення...",330m),

            new Book(48, "ISBN 978-617-12-9372-4", "Стівен Кінг", "Доктор Сон","Минуло багато років після жахливих подій, які відбулися у зловісному готелі в горах... Денні вже дорослий, але привиди й досі не дають йому спокою. Доля зводить його з дівчинкою Аброю, яка має надзвичайні здібності — її «сяйво» сильніше за всіх. Але на неї полюють чудовиська в людській подобі, яким потрібні життя та дар дитини. І тільки Ден здатний захистити її від потвор, якщо переможе власних демонів.",360m),

            new Book(49, "ISBN 9786171506879", "Стівен Кінг", "Долорес Клейборн","<p>Коли Віра Донован, одна з найбагатших і найнепривітніших мешканок острова Літл-Тол у штаті Мен, раптово помирає, підозра одразу ж падає на її економку і доглядальницю Долорес Клейборн. Долорес не вперше стикається з такою недовірою. Місцеві пліткарі звинувачують її у вбивстві власного чоловіка багато років тому. Тоді він загадково загинув під час сонячного затемнення. Тож чи не забагато випадкових смертей на одну людину? <p>"+"Долорес Клейборн починає говорити. Це її натхненна, інтимна та болісна сповідь про найтемніші таємниці, приховані в минулому. У які б шторми її не кидало життя, вона завжди була готова захистити того, кого любить. За будь-яку ціну.",215m),

            new Book(50, "ISBN 978-617-12-9306-9", "Стівен Кінг", "Острів Дума","<p>«Острів Дума» — книга про втрачене кохання й віднайдену дружбу. <p>"+"Трудар-мільйонер дивом вижив у страшній аварії, але залишився калікою з понівеченою психікою. Едгар самотньо мешкає у великому домі на острові Дума в Мексиканській затоці біля узбережжя Флориди. Захоплення малюванням проявляє в ньому паранормальні здібності. Через його феноменальні картини в реальність починає втручатися незбагненна безжальна сила...",430m),

        };

        public Book[] GetAllByIds(IEnumerable<int> bookIds)
        {
            var foundBooks = from book in books
							 join bookId in bookIds on book.Id equals bookId
							 select book;

			return foundBooks.ToArray();
        }

        public Book[] GetAllByIsbn(string isbn)
		{
			return books.Where(book => book.Isbn == isbn)
						.ToArray();
		}

		public Book[] GetAllByTitle(string titlePart)
		{
			if (string.IsNullOrEmpty(titlePart))
			{
				return Array.Empty<Book>();
			}

			return books.Where(book => book.Title.Contains(titlePart, StringComparison.OrdinalIgnoreCase)).ToArray();
		}

		public Book[] GetAllByTitleOrAuthor(string query)
		{
			return books.Where(book => book.Author.Contains(query)
									|| book.Title.Contains(query))
						.ToArray();
		}

		public Book GetById(int id)
		{
            return books.Single(book => book.Id == id);
		}
	}
}