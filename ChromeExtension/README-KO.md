# Wordfall Chrome 확장 프로그램

## 1. Unity WebGL 빌드

Unity에서 `File > Build Profiles > Web > Build`를 실행하고 WebGL 결과 폴더를 준비합니다.

## 2. Firebase 인증 코드 빌드

이 폴더에서 다음 명령을 실행합니다.

```bash
npm install
npm run build "C:\\경로\\UnityWebGL빌드폴더"
```

Unity 빌드 폴더는 `ChromeExtension/game`으로 복사되고 `firebase-bridge.js`가 자동으로 연결됩니다.

## 3. Firebase 콘솔 설정

Authentication > Sign-in method에서 Anonymous와 Google을 사용 설정합니다.

Authentication > Settings > Authorized domains에 배포 후 확장 프로그램 ID를 추가합니다.

```text
chrome-extension://확장프로그램ID
```

## 4. Chrome에서 테스트

`chrome://extensions`로 이동하고 개발자 모드를 켠 뒤 `압축해제된 확장 프로그램을 로드`에서 이 폴더를 선택합니다.

Google 인증이 실패하면 Firebase Authorized domains와 Google 제공업체 설정을 먼저 확인합니다.
