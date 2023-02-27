# autocad-helper

## 기본 사항

이 레포지토리는 오토캐드를 사용할 떄 필요한 기능들을 구현한 C# 코드를 담고 있습니다.

## 기능

다음과 같은 기능이 있습니다.

* 다각형을 원하는 넓이로 수직 또는 수평으로 분할
* 평행한 두 선 또는 중심이 같은 두 호의 중간에 위치하는 선 그리기
  
## 사용 방법

### DLL 로드

1. eunji.dll을 다운로드합니다. 옆의 Releases에도 있고 [bin](https://github.com/ingyer-ks/autocad-helper/tree/main/bin/x64/Debug) 안이나 [obj](https://github.com/ingyer-ks/autocad-helper/tree/main/obj/x64/Debug) 안에서도 찾을 수 있습니다. 다 같습니다.
2. NETLOAD로 eunji.dll을 로드합니다.

### 원하는 넓이로 다각형 분할

1. 상하 두 영역으로 분할하고 싶다면 DV, 좌우 두 영역으로 분할하고 싶다면 DH 명령어를 사용합니다.
2. 분할할 Polyline을 선택합니다.
3. 원하는 면적을 $m^2$ 단위로 입력합니다.
4. 영역을 구분하는 선이 나타납니다.

### 중간선 또는 호 그리기

1. CCX 명령어를 입력합니다.
2. 첫 번째 선 또는 호를 선택합니다.
3. 두 번째 선 또는 호를 선택합니다.
4. 가운데에 위치하는 Xline 또는 호가 그려집니다.

## 버전

DLL 파일은 AutoCAD 2020 버전을 대상으로 만들어졌습니다. 사용하시는 버전에 맞게 ObjectARX SDK를 적용하셔서 빌드하시면 됩니다.

## 기타 사항

제 여자친구인 장은지 님의 불편사항을 해소하기 위해 만들었습니다.

귀찮아서 릴리즈용이 아닌 디버그용으로 빌드하였습니다.

## 라이센스

이 레포지토리는 MIT 라이센스를 따릅니다. 상업적 이용, 수정, 배포, 개인적 이용이 가능하나, 라이센스 및 저작권을 고지하여야 합니다. 또한 이 레포지토리를 사용함으로 인해 발생하는 문제에 대해서 책임을 지지 않으며 어떠한 보증 또한 없습니다.

## Background

This repository stores C# codes which provides additional functions on Autocad.

## Functions

This repository implements these functions:

* Dividing an polygon horizontally or vertically with desired area.
* Drawing a line exactly middle of two parallel lines or a arc exactly middle of two arcs which share same center.

## How to Use

### DLL Loading

1. Download eunji.dll. You can find it at Release or [bin](https://github.com/ingyer-ks/autocad-helper/tree/main/bin/x64/Debug) or [obj](https://github.com/ingyer-ks/autocad-helper/tree/main/obj/x64/Debug). These are same files.
2. Load eunji.dll by NETLOAD.

### Dividing Polygon by desired area

1. If you want to divide by vertical line, use command DV and for dividing by horizontal line, use command DH.
2. Select the Polyline which consists of the division which will have the area you want.
3. Enter desired area in $m^2$ dimension.
4. Now you can see the division.

### Drawing a line or an arc passing through the middle

1. Use CCX command.
2. Select first line or arc.
3. Select second line or arc.
4. A new Xline or arc which passes through the middle appears.

## Version

The DLL file targets AutoCAD 2020. If you use other version, you need corresponding ObjectARX SDK.

## Miscellaneous

This is dedicated to Jang Eunji who is my girlfriend.

My laziness has led me to build for Debug, not Release.

## License

This repository is licensed under the terms of the MIT license.
