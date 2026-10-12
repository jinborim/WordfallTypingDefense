<<<<<<< HEAD
# Wordfall Typing Defense

한글과 영어 단어를 입력해 떨어지는 단어를 막는 2D 픽셀 타자 방어 게임입니다.
Unity WebGL 빌드를 목표로 하며, Chrome 웹 스토어 출시를 준비하고 있습니다.

## 주요 기능

- 한글 / 영어 언어 선택
- 한국어와 영어 타자 모드
- 혼합, 한글, 영어, 60초 도전, 오늘의 도전 모드
- 쉬움, 보통, 어려움 난이도
- 떨어지는 단어를 입력하는 타자 방어 시스템
- 콤보, 점수, 목숨, 웨이브 시스템
- 골든 단어, 폭탄 단어, 회복 단어, 보스 단어
- F1 빙결 스킬, F2 화면 정리 스킬
- 일시정지 및 입력창 자동 포커스
- 픽셀 배경, 픽셀 버튼, 한글 지원 픽셀 폰트

## 실행 방법

1. Unity Hub에서 Unity `6000.3.24f1`로 프로젝트를 엽니다.
2. `Assets/Scenes`에 씬이 없다면 빈 씬을 열어도 됩니다.
3. Play 버튼을 눌러 실행합니다.
4. 시작 화면에서 언어, 난이도, 게임 모드를 선택합니다.

게임 UI는 `WordfallPrototype` 스크립트가 실행될 때 자동으로 생성됩니다.

## 조작법

| 키 / 기능 | 설명 |
|---|---|
| 키보드 입력 | 화면에 떨어지는 단어 입력 |
| Enter | 입력한 단어 제출 |
| ESC | 일시정지 / 계속하기 |
| F1 | 5초 동안 단어 정지 |
| F2 | 화면의 단어 제거 |

## 프로젝트 구조

```text
Assets/
├─ Fonts/                 폰트 라이선스
├─ Resources/
│  ├─ Fonts/              런타임에서 불러오는 한글 픽셀 폰트
│  └─ Pixel/              픽셀 배경, 로고, 버튼 이미지
└─ Scripts/
   └─ WordfallPrototype.cs 게임 핵심 로직과 UI 생성
Packages/                 Unity 패키지 설정
ProjectSettings/          Unity 프로젝트 설정
```

## 픽셀 폰트 및 이미지

한글 표시를 위해 Mulmaru 픽셀 폰트를 사용합니다.
폰트 라이선스는 `Assets/Fonts/Mulmaru-LICENSE.txt`를 확인하세요.

픽셀 이미지 파일은 다음 위치에 있습니다.

- `Assets/Resources/Pixel/wordfall-background.png`
- `Assets/Resources/Pixel/wordfall-shield.png`
- `Assets/Resources/Pixel/wordfall-button.png`

## WebGL 빌드

1. `File > Build Profiles`를 엽니다.
2. Web 플랫폼 또는 WebGL 프로필을 선택합니다.
3. 플랫폼을 전환합니다.
4. `Build` 또는 `Build And Run`을 실행합니다.

빌드 결과물은 GitHub에 올리기 전에 별도 배포 폴더에 보관하는 것을 권장합니다.

## GitHub 커밋 예시

```powershell
git add Assets Packages ProjectSettings README.md
git commit -m "픽셀 UI와 언어 선택 기능 추가"
git push origin main
```

## 개발 메모

- `Library`, `Temp`, `Logs`, `Obj` 폴더는 GitHub에 올리지 않습니다.
- 새 이미지는 `Assets/Resources/Pixel`에 넣습니다.
- 런타임에서 사용할 폰트는 `Assets/Resources/Fonts`에 넣습니다.
- 기능을 추가할 때는 README의 주요 기능 목록과 조작법도 함께 업데이트합니다.
=======
# WordfallTypingDefense
떨어지는 한글과 영어 단어를 빠르게 입력해 막아내는 타자 디펜스 게임
>>>>>>> 3276d1aebe4f4edf7866e1a15cae952cee81578d
