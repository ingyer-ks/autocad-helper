# autocad
## 기본 사항
이 레포지토리는 오토캐드를 사용할 떄 필요한 기능들을 구현한 C# 코드를 저장하는 레포지토리입니다.

## 기능
현재는 어떤 폐곡선을 원하는 면적으로 위아래, 혹은 좌우로 분할하는 기능이 있습니다.

## 사용 방법
1. NETLOAD로 eunji.dll을 로드합니다.
2. 상하 두 영역으로 분할하고 싶다면 DIVVERT, 좌우 두 영역으로 분할하고 싶다면 DIVHORIZ 명령어를 사용합니다.
3. 분할할 Polyline을 선택합니다.
4. 원하는 면적을 m^2 단위로 입력합니다.
5. 영역을 구분하는 선이 나타납니다.

## 버전
DLL 파일은 AutoCAD 2020 버전을 대상으로 만들어졌습니다. 사용하시는 버전에 맞게 ObjectARX SDK를 적용하셔서 빌드하시면 됩니다.
## 기타 사항
제 여자친구인 장은지 님의 불편사항을 해소하기 위해 만들었습니다.

## Background
This repository stores C# codes which provides additional functions on Autocad.

## Functions
At this moment, you can divide a polygon into two area vertically or horizontally.

## How to Use
1. Load eunji.dll by NETLOAD.
2. If you want to divde vertically(upper/lower area) use DIVVERT, and if you want to divide horizontally(left/right area) use DIVHORIZ.
3. Select the Polyline you want to divide.
4. Enter desired area in m^2 dimension.
5. Now you can see the division.

## Version
The DLL file targets AutoCAD 2020. If you use 
## Miscellaneous
This is dedicated to Jang Eunji who is my girlfriend.