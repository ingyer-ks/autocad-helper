# autocad
## 기본 사항
이 레포지토리는 오토캐드를 사용할 떄 필요한 기능들을 구현한 C# 코드를 저장하는 레포지토리입니다.

## 기능
다음과 같은 기능이 있습니다.
* 다각형을 원하는 넓이로 수직 또는 수평으로 분할
* 평행한 두 선의 중간에 위치하는 선 그리기
## 사용 방법
* DLL 로드
1. bin/Debug 안의 eunji.dll을 다운로드합니다.
2. NETLOAD로 eunji.dll을 로드합니다.

* 다각형 분할
1. 상하 두 영역으로 분할하고 싶다면 DV, 좌우 두 영역으로 분할하고 싶다면 DH 명령어를 사용합니다.
2. 분할할 Polyline을 선택합니다.
3. 원하는 면적을 m^2 단위로 입력합니다.
4. 영역을 구분하는 선이 나타납니다.

* 중간선 그리기
1. CCX 명령어를 입력합니다.
2. 안쪽 선을 선택합니다.
3. 바깥쪽 선을 선택합니다.
4. 안쪽 선과 바깥쪽 선의 가운데를 지나는 Xline이 만들어집니다.

## 버전
DLL 파일은 AutoCAD 2020 버전을 대상으로 만들어졌습니다. 사용하시는 버전에 맞게 ObjectARX SDK를 적용하셔서 빌드하시면 됩니다.
## 기타 사항
제 여자친구인 장은지 님의 불편사항을 해소하기 위해 만들었습니다.

## 라이센스
이 레포지토리는 MIT 라이센스를 따릅니다. 상업적 이용, 수정, 배포, 개인적 이용이 가능하나, 라이센스 및 저작권을 고지하여야 합니다. 또한 이 레포지토리를 사용함으로 인해 발생하는 문제에 대해서 책임을 지지 않으며 어떠한 보증 또한 없습니다.

## Background
This repository stores C# codes which provides additional functions on Autocad.

## Functions
This repository implements these functions:
* Dividing an polygon horizontally or vertically with desired area.
* Drawing a line exactly middle of two parallel lines.

## How to Use
* DLL Load
1. Download eunji.dll from bin/Debug directory.
2. Load eunji.dll by NETLOAD.

* Dividing Polygon
1. If you want to divde vertically(upper/lower area) use command DV, and if you want to divide horizontally(left/right area) use command DH.
2. Select the Polyline you want to divide.
3. Enter desired area in m^2 dimension.
4. Now you can see the division.

* Drawing a line at the middle of two parallel lines
1. Use CCX command.
2. Select the inner line.
3. Select the outer line.
4. A new Xline which passes through the middle point of both lines.
## Version
The DLL file targets AutoCAD 2020. If you use other version, you need corresponding ObjectARX SDK.
## Miscellaneous
This is dedicated to Jang Eunji who is my girlfriend.

## License
This repository is licensed under the terms of the MIT license.