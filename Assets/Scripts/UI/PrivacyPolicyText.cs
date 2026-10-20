using UnityEngine;

public sealed partial class WordfallPrototype
{
    private void ApplyExpandedPrivacyPolicy(bool korean)
    {
        if (privacyMessageText == null) return;
        privacyMessageText.text = korean ? GetKoreanPrivacyPolicy() : GetEnglishPrivacyPolicy();
    }

    private static string GetKoreanPrivacyPolicy()
    {
        return @"시행일: 2026년 10월 21일

1. 개인정보처리자 및 문의
서비스명: Wordfall
개인정보처리자: Wordfall 개발자
문의처: Chrome Web Store 게시 페이지에 표시된 개발자 문의처

2. 처리 목적
게임 계정 로그인과 계정별 데이터 분리, 프로필 및 닉네임 제공, 닉네임 중복 확인, 게임 진행·통계·상점 기능 제공, 서비스 보안과 오류 대응을 위해 개인정보를 처리합니다.

3. 처리하는 개인정보
• 게스트 로그인: Firebase가 발급하는 익명 사용자 ID(UID)
• Google 로그인: Firebase 사용자 ID, Google에서 제공하는 표시 이름 및 인증에 필요한 계정 정보
• 프로필 정보: 사용자가 입력한 닉네임과 선택 캐릭터
• 게임 정보: 코인, 변경권, 보유 캐릭터, 게임 통계, 최고 점수, 언어 및 설정
• 닉네임 중복 확인: 닉네임, 소유 계정 UID, 등록·변경 시각
• 자동 수집·기술 정보: 로그인 상태, 브라우저 저장소 또는 PlayerPrefs에 저장되는 세션·설정 정보, Firebase의 오류·보안 로그

4. 보유 및 이용 기간
로컬 게임 데이터는 브라우저 저장 데이터 또는 앱 데이터를 삭제하거나 회원 탈퇴를 처리할 때까지 보유합니다.
닉네임 예약 정보는 닉네임을 변경하거나 계정을 탈퇴할 때 삭제하며, 법령상 보존이 필요한 경우 해당 기간 동안 보존합니다.
Firebase 인증 정보는 계정 삭제 요청 또는 Firebase 및 관계 법령의 보존 정책에 따라 처리합니다.

5. 개인정보의 제공 및 처리위탁
Google Firebase를 인증, 닉네임 중복 확인 및 서비스 운영을 위한 클라우드 서비스 제공자로 이용합니다. 필요한 범위를 넘어 개인정보를 판매하거나 맞춤형 광고에 이용하지 않습니다.
Firebase는 국외 서버에서 정보를 처리할 수 있으며, 서비스 제공에 필요한 범위에서 정보가 국외로 이전될 수 있습니다.
Firebase 개인정보 보호 안내: https://firebase.google.com/support/privacy/
Google 개인정보처리방침: https://policies.google.com/privacy

6. 이용자의 권리 및 행사 방법
이용자는 자신의 개인정보에 대해 열람, 정정, 삭제, 처리정지를 요청할 수 있습니다.
앱의 회원 탈퇴 기능을 이용하거나 위 문의처로 요청할 수 있습니다. 본인 확인에 필요한 최소한의 정보 제출을 요청할 수 있습니다.

7. 회원 탈퇴 및 삭제
회원 탈퇴 시 계정에 연결된 로컬 게임 데이터와 닉네임 예약 정보를 삭제합니다. Firebase 인증 계정 자체의 삭제 여부는 로그인 제공자와 Firebase 계정 상태에 따라 별도 처리가 필요할 수 있으며, 삭제 요청이 접수되면 확인 후 관련 계정 정보를 처리합니다.

8. 안전성 확보 조치
인증 서비스의 접근 통제, Firestore 보안 규칙, HTTPS 전송, 계정별 데이터 분리 및 필요한 권한만 부여하는 방식으로 개인정보를 보호합니다.

9. 아동·청소년
법정대리인의 동의가 필요한 연령의 이용자는 법정대리인의 동의를 받아 서비스를 이용해야 합니다.

10. 처리방침의 변경
법령, 서비스 기능 또는 처리 항목이 변경되면 앱 내 공지와 업데이트된 처리방침으로 안내합니다.";
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
Firebase may process information on servers outside your country, and information may be transferred internationally as needed to provide the service.
Firebase Privacy: https://firebase.google.com/support/privacy/
Google Privacy Policy: https://policies.google.com/privacy

6. Your rights
You may request access, correction, deletion, or restriction of processing. Use the in-game account deletion feature or contact the developer through the contact shown on the Chrome Web Store listing. We may request information needed to verify the request.

7. Account deletion
Account deletion removes the account-linked local game data and nickname reservation. Deletion of the underlying Firebase authentication account may require separate handling depending on the sign-in provider and Firebase account state; requests will be reviewed and processed for the relevant account information.

8. Security
We use access controls, Firestore Security Rules, HTTPS transmission, account-separated storage, and least-privilege access where appropriate.

9. Children
Users who require consent under applicable law must obtain consent from a legal representative before using the service.

10. Changes
We will announce material changes through the app and publish an updated version of this policy when the law, service, or processing activities change.";
    }
}
