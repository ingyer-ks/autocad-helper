# autocad
## 기본 사항
이 레포지토리는 오토캐드를 사용할 떄 필요한 기능들을 구현한 C# 코드를 저장하는 레포지토리입니다.

## 기능
현재는 어떤 폐곡선을 원하는 면적으로 위아래, 혹은 좌우로 분할하는 기능이 있습니다.

## 사용 방법
1. bin/Debug 안의 eunji.dll을 다운로드합니다.
2. NETLOAD로 eunji.dll을 로드합니다.
3. 상하 두 영역으로 분할하고 싶다면 DV, 좌우 두 영역으로 분할하고 싶다면 DH 명령어를 사용합니다.
4. 분할할 Polyline을 선택합니다.
5. 원하는 면적을 m^2 단위로 입력합니다.
6. 영역을 구분하는 선이 나타납니다.

## 버전
DLL 파일은 AutoCAD 2020 버전을 대상으로 만들어졌습니다. 사용하시는 버전에 맞게 ObjectARX SDK를 적용하셔서 빌드하시면 됩니다.
## 기타 사항
제 여자친구인 장은지 님의 불편사항을 해소하기 위해 만들었습니다.

## 라이센스
이 레포지토리는 MIT 라이센스를 따릅니다. 상업적 이용, 수정, 배포, 개인적 이용이 가능하나, 라이센스 및 저작권을 고지하여야 합니다. 또한 이 레포지토리를 사용함으로 인해 발생하는 문제에 대해서 책임을 지지 않으며 어떠한 보증 또한 없습니다.

## Background
This repository stores C# codes which provides additional functions on Autocad.

## Functions
At this moment, you can divide a polygon into two area vertically or horizontally.

## How to Use
1. Download eunji.dll from bin/Debug directory.
2. Load eunji.dll by NETLOAD.
3. If you want to divde vertically(upper/lower area) use command DV, and if you want to divide horizontally(left/right area) use command DH.
4. Select the Polyline you want to divide.
5. Enter desired area in m^2 dimension.
6. Now you can see the division.

## Version
The DLL file targets AutoCAD 2020. If you use other version, you need corresponding ObjectARX SDK.
## Miscellaneous
This is dedicated to Jang Eunji who is my girlfriend.

## License
This repository is licensed under the terms of the MIT license.