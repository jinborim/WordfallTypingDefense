using UnityEngine;

public sealed partial class WordfallPrototype
{
    private void ApplyExpandedPrivacyPolicy(bool korean)
    {
        if (privacyMessageText == null) return;
        privacyMessageText.text = korean ? GetKoreanPrivacyPolicyUtf8() : GetEnglishPrivacyPolicy();
        privacyMessageText.gameObject.SetActive(true);
        privacyMessageText.enabled = true;
    }

    private static string GetKoreanPrivacyPolicyUtf8()
    {
        return System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String("7Iuc7ZaJ7J28OiAyMDI264WEIDEw7JuUIDIx7J28CgoxLiDqsJzsnbjsoJXrs7TsspjrpqzsnpAg67CPIOusuOydmArshJzruYTsiqTrqoU6IFdvcmRmYWxsCuqwnOyduOygleuztOyymOumrOyekDogV29yZGZhbGwg6rCc67Cc7J6QCuusuOydmOyymDogQ2hyb21lIFdlYiBTdG9yZSDtjpjsnbTsp4Dsl5Ag7ZGc7Iuc65CcIOqwnOuwnOyekCDrrLjsnZjsspgKCjIuIOyymOumrCDrqqnsoIEK6rKM7J6EIOqzhOyglSDroZzqt7jsnbjqs7wg6rOE7KCV67OEIOuNsOydtO2EsCDrtoTrpqwsIO2UhOuhnO2VhCDrsI8g64uJ64Sk7J6EIOygnOqztSwg64uJ64Sk7J6EIOykkeuztSDtmZXsnbgsIOqyjOyehCDsp4TtlonCt+2GteqzhMK37IOB7KCQIOq4sOuKpSDsoJzqs7UsIOyEnOu5hOyKpCDrs7TslYjqs7wg7Jik66WYIOuMgOydkeydhCDsnITtlbQg6rCc7J247KCV67O066W8IOyymOumrO2VqeuLiOuLpC4KCjMuIOyymOumrO2VmOuKlCDqsJzsnbjsoJXrs7QK4oCiIOqyjOyKpO2KuCDroZzqt7jsnbg6IEZpcmViYXNl6rCAIOuwnOq4ie2VmOuKlCDsnbXrqoUg7IKs7Jqp7J6QIElEKFVJRCkK4oCiIEdvb2dsZSDroZzqt7jsnbg6IEZpcmViYXNlIOyCrOyaqeyekCBJRCwgR29vZ2xl7JeQ7IScIOygnOqzte2VmOuKlCDtkZzsi5wg7J2066aEIOuwjyDsnbjspp3sl5Ag7ZWE7JqU7ZWcIOqzhOyglSDsoJXrs7QK4oCiIO2UhOuhnO2VhCDsoJXrs7Q6IOyCrOyaqeyekOqwgCDsnoXroKXtlZwg64uJ64Sk7J6E6rO8IOyEoO2DnSDsupDrpq3thLAK4oCiIOqyjOyehCDsoJXrs7Q6IOy9lOyduCwg67OA6rK96raMLCDrs7TsnKAg7LqQ66at7YSwLCDqsozsnoQg7Ya16rOELCDstZzqs6Ag7KCQ7IiYLCDslrjslrQg67CPIOyEpOyglQrigKIg64uJ64Sk7J6EIOykkeuztSDtmZXsnbg6IOuLieuEpOyehCwg7IaM7JygIOqzhOyglSBVSUQsIOuTseuhncK367OA6rK9IOyLnOqwgQrigKIg7J6Q64+ZIOyImOynkcK36riw7IigIOygleuztDog66Gc6re47J24IOyDge2DnCwg67iM65287Jqw7KCAIOyggOyepeyGjCDrmJDripQgUGxheWVyUHJlZnPsl5Ag7KCA7J6l65CY64qUIOyEuOyFmMK37ISk7KCVIOygleuztCwgRmlyZWJhc2Ug7Jik66WYwrfrs7TslYgg66Gc6re4Cgo0LiDrs7TsnKAg67CPIOydtOyaqSDquLDqsIQK66Gc7LusIOqyjOyehCDrjbDsnbTthLDripQg67iM65287Jqw7KCAIOuYkOuKlCDslbEg642w7J207YSw66W8IOyCreygnO2VmOqxsOuCmCDtmozsm5Ag7YOI7Ye066W8IOyymOumrO2VoCDrlYzquYzsp4Ag67O07Jyg7ZWp64uI64ukLgrri4nrhKTsnoQg7JiI7JW9IOygleuztOuKlCDri4nrhKTsnoQg67OA6rK9IOuYkOuKlCDqs4TsoJUg7IKt7KCcIOyLnCDsgq3soJztlanri4jri6QuIOuyleugueyXkCDrlLDrnbwg67O07KG07J20IO2VhOyalO2VnCDqsr3smrDsl5DripQg7ZW064u5IOq4sOqwhCDrj5nslYgg67O07KG07ZWp64uI64ukLgpGaXJlYmFzZSDsnbjspp0g7KCV67O064qUIOqzhOyglSDsgq3soJwg7JqU7LKtLCBGaXJlYmFzZSDsoJXssYUg67CPIOq0gOqzhCDrspXroLnsl5Ag65Sw6528IOyymOumrO2VqeuLiOuLpC4KCjUuIOqwnOyduOygleuztOydmCDsoJzqs7Ug67CPIOyymOumrOychO2DgQpHb29nbGUgRmlyZWJhc2Xrpbwg7J247KadLCDri4nrhKTsnoQg7KSR67O1IO2ZleyduCDrsI8g7ISc67mE7IqkIOyatOyYgeydhCDsnITtlZwg7YG065287Jqw65OcIOyEnOu5hOyKpCDsoJzqs7XsnpDroZwg7J207Jqp7ZWp64uI64ukLgrtlYTsmpTtlZwg67KU7JyE66W8IOuEmOyWtCDqsJzsnbjsoJXrs7Trpbwg7YyQ66ek7ZWY6rGw64KYIOunnuy2pO2YlSDqtJHqs6Dsl5Ag7J207Jqp7ZWY7KeAIOyViuyKteuLiOuLpC4gRmlyZWJhc2XripQg7ISc67mE7IqkIOygnOqzteydhCDsnITtlbQg6rWt7Jm4IOyEnOuyhOyXkOyEnCDsoJXrs7Trpbwg7LKY66as7ZWgIOyImCDsnojsirXri4jri6QuCgpGaXJlYmFzZSDqsJzsnbjsoJXrs7Qg67O07Zi4IOyViOuCtDogaHR0cHM6Ly9maXJlYmFzZS5nb29nbGUuY29tL3N1cHBvcnQvcHJpdmFjeS8KR29vZ2xlIOqwnOyduOygleuztOyymOumrOuwqey5qDogaHR0cHM6Ly9wb2xpY2llcy5nb29nbGUuY29tL3ByaXZhY3kKCjYuIOydtOyaqeyekOydmCDqtozrpqwg67CPIO2WieyCrCDrsKnrspUK7J207Jqp7J6Q64qUIOyekOyLoOydmCDqsJzsnbjsoJXrs7Tsl5Ag64yA7ZW0IOyXtOuejCwg7KCV7KCVLCDsgq3soJwsIOyymOumrOygleyngOulvCDsmpTssq3tlaAg7IiYIOyeiOyKteuLiOuLpC4g6rKM7J6EIOuCtCDtmozsm5Ag7YOI7Ye0IOq4sOuKpeydhCDsnbTsmqntlZjqsbDrgpggQ2hyb21lIFdlYiBTdG9yZeyXkCDtkZzsi5zrkJwg6rCc67Cc7J6QIOusuOydmOyymOuhnCDsmpTssq3tlaAg7IiYIOyeiOyKteuLiOuLpC4KCjcuIO2ajOybkCDtg4jth7Qg67CPIOyCreygnArtmozsm5Ag7YOI7Ye0IOyLnCDqs4TsoJXsl5Ag7Jew6rKw65CcIOuhnOy7rCDqsozsnoQg642w7J207YSw7JmAIOuLieuEpOyehCDsmIjslb0g7KCV67O066W8IOyCreygnO2VqeuLiOuLpC4gRmlyZWJhc2Ug7J247KadIOqzhOyglSDsgq3soJzripQg66Gc6re47J24IOygnOqzteyXheyytOyZgCDqs4TsoJUg7IOB7YOc7JeQIOuUsOudvCDrs4Trj4Qg7LKY66as6rCAIO2VhOyalO2VoCDsiJgg7J6I7Iq164uI64ukLgoKOC4g7JWI7KCE7ISxIO2ZleuztCDsobDsuZgK7J247KadIOyEnOu5hOyKpCDsoJHqt7wg7Ya17KCcLCBGaXJlc3RvcmUg67O07JWIIOq3nOy5mSwgSFRUUFMg7KCE7IahLCDqs4TsoJXrs4Qg642w7J207YSwIOu2hOumrCDrsI8g7ZWE7JqU7ZWcIOq2jO2VnOunjCDrtoDsl6ztlZjripQg67Cp7Iud7Jy866GcIOqwnOyduOygleuztOulvCDrs7TtmLjtlanri4jri6QuCgo5LiDslYTrj5nCt+yyreyGjOuFhArqtIDroKgg67KV66C57IOBIOuztO2YuOyekCDrj5nsnZjqsIAg7ZWE7JqU7ZWcIOydtOyaqeyekOuKlCDrspXsoJXrjIDrpqzsnbjsnZgg64+Z7J2Y66W8IOuwm+ydgCDtm4Qg7ISc67mE7Iqk66W8IOydtOyaqe2VtOyVvCDtlanri4jri6QuCgoxMC4g7LKY66as67Cp7LmoIOuzgOqyvQrrspXroLksIOyEnOu5hOyKpCDquLDriqUg65iQ64qUIOqwnOyduOygleuztCDsspjrpqwg7ZWt66qp7J20IOuzgOqyveuQmOuptCDslbEg6rO17KeA7JmAIOyXheuNsOydtO2KuOuQnCDsspjrpqzrsKnsuajsnYQg7Ya17ZW0IOyViOuCtO2VqeuLiOuLpC4="));
    }

    private static string GetKoreanPrivacyPolicyFixed()
    {
        return "시행일: 2026년 10월 21일\n\n"
            + "1. 개인정보처리자 및 문의\n"
            + "서비스명: Wordfall\n"
            + "개인정보처리자: Wordfall 개발자\n"
            + "문의처: Chrome Web Store 페이지에 표시된 개발자 문의처\n\n"
            + "2. 처리 목적\n"
            + "게임 계정 로그인과 계정별 데이터 분리, 프로필 및 닉네임 제공, 닉네임 중복 확인, 게임 진행·통계·상점 기능 제공, 서비스 보안과 오류 대응을 위해 개인정보를 처리합니다.\n\n"
            + "3. 처리하는 개인정보\n"
            + "• 게스트 로그인: Firebase가 발급하는 익명 사용자 ID(UID)\n"
            + "• Google 로그인: Firebase 사용자 ID, Google에서 제공하는 표시 이름 및 인증에 필요한 계정 정보\n"
            + "• 프로필 정보: 사용자가 입력한 닉네임과 선택 캐릭터\n"
            + "• 게임 정보: 코인, 변경권, 보유 캐릭터, 게임 통계, 최고 점수, 언어 및 설정\n"
            + "• 닉네임 중복 확인: 닉네임, 소유 계정 UID, 등록·변경 시각\n"
            + "• 자동 수집·기술 정보: 로그인 상태, 브라우저 저장소 또는 PlayerPrefs에 저장되는 세션·설정 정보, Firebase 오류·보안 로그\n\n"
            + "4. 보유 및 이용 기간\n"
            + "로컬 게임 데이터는 브라우저 또는 앱 데이터를 삭제하거나 회원 탈퇴를 처리할 때까지 보유합니다.\n"
            + "닉네임 예약 정보는 닉네임 변경 또는 계정 삭제 시 삭제합니다. 법령에 따라 보존이 필요한 경우에는 해당 기간 동안 보존합니다.\n"
            + "Firebase 인증 정보는 계정 삭제 요청, Firebase 정책 및 관계 법령에 따라 처리합니다.\n\n"
            + "5. 개인정보의 제공 및 처리위탁\n"
            + "Google Firebase를 인증, 닉네임 중복 확인 및 서비스 운영을 위한 클라우드 서비스 제공자로 이용합니다.\n"
            + "필요한 범위를 넘어 개인정보를 판매하거나 맞춤형 광고에 이용하지 않습니다. Firebase는 서비스 제공을 위해 국외 서버에서 정보를 처리할 수 있습니다.\n\n"
            + "Firebase 개인정보 보호 안내: https://firebase.google.com/support/privacy/\n"
            + "Google 개인정보처리방침: https://policies.google.com/privacy\n\n"
            + "6. 이용자의 권리 및 행사 방법\n"
            + "이용자는 자신의 개인정보에 대해 열람, 정정, 삭제, 처리정지를 요청할 수 있습니다. 게임 내 회원 탈퇴 기능을 이용하거나 Chrome Web Store에 표시된 개발자 문의처로 요청할 수 있습니다.\n\n"
            + "7. 회원 탈퇴 및 삭제\n"
            + "회원 탈퇴 시 계정에 연결된 로컬 게임 데이터와 닉네임 예약 정보를 삭제합니다. Firebase 인증 계정 삭제는 로그인 제공업체와 계정 상태에 따라 별도 처리가 필요할 수 있습니다.\n\n"
            + "8. 안전성 확보 조치\n"
            + "인증 서비스 접근 통제, Firestore 보안 규칙, HTTPS 전송, 계정별 데이터 분리 및 필요한 권한만 부여하는 방식으로 개인정보를 보호합니다.\n\n"
            + "9. 아동·청소년\n"
            + "관련 법령상 보호자 동의가 필요한 이용자는 법정대리인의 동의를 받은 후 서비스를 이용해야 합니다.\n\n"
            + "10. 처리방침 변경\n"
            + "법령, 서비스 기능 또는 개인정보 처리 항목이 변경되면 앱 공지와 업데이트된 처리방침을 통해 안내합니다.";
    }

    private static string GetKoreanPrivacyPolicy()
    {
        return @"시행일: 2026년 10월 21일

1. 개인정보처리자 및 문의
서비스명: Wordfall
개인정보처리자: Wordfall 개발자
문의처: Chrome Web Store 페이지에 표시된 개발자 문의처

2. 처리 목적
게임 계정 로그인과 계정별 데이터 분리, 프로필 및 닉네임 제공, 닉네임 중복 확인, 게임 진행·통계·상점 기능 제공, 서비스 보안과 오류 대응을 위해 개인정보를 처리합니다.

3. 처리하는 개인정보
• 게스트 로그인: Firebase가 발급하는 익명 사용자 ID(UID)
• Google 로그인: Firebase 사용자 ID, Google에서 제공하는 표시 이름 및 인증에 필요한 계정 정보
• 프로필 정보: 사용자가 입력한 닉네임과 선택 캐릭터
• 게임 정보: 코인, 변경권, 보유 캐릭터, 게임 통계, 최고 점수, 언어 및 설정
• 닉네임 중복 확인: 닉네임, 소유 계정 UID, 등록·변경 시각
• 자동 수집·기술 정보: 로그인 상태, 브라우저 저장소 또는 PlayerPrefs에 저장되는 세션·설정 정보, Firebase 오류·보안 로그

4. 보유 및 이용 기간
로컬 게임 데이터는 브라우저 또는 앱 데이터를 삭제하거나 회원 탈퇴를 처리할 때까지 보유합니다.
닉네임 예약 정보는 닉네임 변경 또는 계정 삭제 시 삭제합니다. 법령에 따라 보존이 필요한 경우에는 해당 기간 동안 보존합니다.
Firebase 인증 정보는 계정 삭제 요청, Firebase 정책 및 관계 법령에 따라 처리합니다.

5. 개인정보의 제공 및 처리위탁
Google Firebase를 인증, 닉네임 중복 확인 및 서비스 운영을 위한 클라우드 서비스 제공자로 이용합니다.
필요한 범위를 넘어 개인정보를 판매하거나 맞춤형 광고에 이용하지 않습니다.
Firebase는 서비스 제공을 위해 국외 서버에서 정보를 처리할 수 있습니다.

Firebase 개인정보 보호 안내: https://firebase.google.com/support/privacy/
Google 개인정보처리방침: https://policies.google.com/privacy

6. 이용자의 권리 및 행사 방법
이용자는 자신의 개인정보에 대해 열람, 정정, 삭제, 처리정지를 요청할 수 있습니다.
게임 내 회원 탈퇴 기능을 이용하거나 Chrome Web Store에 표시된 개발자 문의처로 요청할 수 있습니다.
요청자 확인을 위해 필요한 최소한의 정보를 요청할 수 있습니다.

7. 회원 탈퇴 및 삭제
회원 탈퇴 시 계정에 연결된 로컬 게임 데이터와 닉네임 예약 정보를 삭제합니다.
로그인 제공업체에 따른 Firebase 인증 계정의 삭제는 별도 처리가 필요할 수 있으며, 요청을 확인한 뒤 관련 계정 정보를 처리합니다.

8. 안전성 확보 조치
인증 서비스 접근 통제, Firestore 보안 규칙, HTTPS 전송, 계정별 데이터 분리 및 필요한 권한만 부여하는 방식으로 개인정보를 보호합니다.

9. 아동·청소년
관련 법령상 보호자 동의가 필요한 이용자는 법정대리인의 동의를 받은 후 서비스를 이용해야 합니다.

10. 처리방침 변경
법령, 서비스 기능 또는 개인정보 처리 항목이 변경되면 앱 공지와 업데이트된 처리방침을 통해 안내합니다.";
    }

    private static string GetEnglishPrivacyPolicy()
    {
        return @"Effective date: October 21, 2026

1. Controller and contact
Service: Wordfall
Controller: Wordfall developer
Contact: the developer contact shown on the Chrome Web Store listing

2. Purposes
We process information to provide sign-in, account-separated game data, profiles, nickname availability checks, gameplay, statistics, shop features, security, and error handling.

3. Information processed
• Guest sign-in: the anonymous Firebase user ID (UID)
• Google sign-in: Firebase user ID, display name, and account information provided by Google that is needed for authentication
• Profile information: the nickname and character selected by the user
• Game information: coins, nickname tickets, owned characters, game statistics, high score, language, and settings
• Nickname reservation: nickname, owning account UID, and registration or change time
• Technical information: sign-in state, browser storage or PlayerPrefs data used for sessions and settings, and Firebase security or error logs

4. Retention
Local game data is retained until browser or app data is cleared or account deletion is processed.
Nickname reservations are released when a nickname is changed or an account is deleted, except where a longer retention period is required by law.
Firebase authentication information is handled according to an account deletion request, Firebase policies, and applicable law.

5. Sharing and processing by providers
We use Google Firebase for authentication, nickname availability checks, and cloud service operations. We do not sell personal information or use it for targeted advertising.
Firebase may process information on servers outside your country as needed to provide the service.

Firebase Privacy: https://firebase.google.com/support/privacy/
Google Privacy Policy: https://policies.google.com/privacy

6. Your rights
You may request access, correction, deletion, or restriction of processing. Use the in-game account deletion feature or contact the developer through the contact shown on the Chrome Web Store listing.

7. Account deletion
Account deletion removes the account-linked local game data and nickname reservation. Deletion of the underlying Firebase authentication account may require separate handling depending on the sign-in provider and Firebase account state.

8. Security
We use access controls, Firestore Security Rules, HTTPS transmission, account-separated storage, and least-privilege access where appropriate.

9. Children
Users who require consent under applicable law must obtain consent from a legal representative before using the service.

10. Changes
We will announce material changes through the app and publish an updated version of this policy when the law, service, or processing activities change.";
    }
}
