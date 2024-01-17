# The Legend of Elemental Guy

## Introductions
몬스터들이 바글거리는 미지의 던전에서 살아남아 보물 상자를 쟁취하세요!
상황에 맞게 적절한 원소 스킬을 사용해서 적들을 무찌르세요.
혼자서 즐길 수 있는 도트 방식의 SRPG입니다.

## Developers
- 김선오 - 전투 컨셉 기획, 전투 Scene 개발, Transition 및 BGM 적용
- 손다윤 - 게임 컨셉 기획, 맵 Scene 개발, Animation 및 Asset 적용

## Development Tools
- Unity

## Game Play
### Main Scene
![스크린샷 2024-01-17 20 05 48](https://github.com/sunohkim/Madcamp_Week3_RPG/assets/37200748/16f9b925-5af2-4314-ae49-80a57ee2d778)

아무 키를 누르면 게임이 시작됩니다.

### Map Scene
![스크린샷 2024-01-17 20 07 21](https://github.com/sunohkim/Madcamp_Week3_RPG/assets/37200748/7b365979-b923-415a-b6ce-218ff97b1d12)

맵을 돌아다니면서 적과 만나서 싸우거나, 보물 상자를 획득할 수 있습니다.
키보드 방향키로 플레이어의 움직임을 컨트롤할 수 있습니다.
적과 만나게 되면 Battle Scene으로 넘어가게 됩니다.

### Battle Scene
![스크린샷 2024-01-17 20 09 01](https://github.com/sunohkim/Madcamp_Week3_RPG/assets/37200748/92b23e8d-bc3d-478e-8b90-e26035d60f0c)

턴제 방식으로 플레이어와 몬스터가 싸웁니다.
플레이어 턴에서는 기본 공격을 하거나, 스킬을 사용할 수 있습니다.
스킬은 총 3개가 존재하며, 각 스킬 별 원소 속성, 효과, 쿨타임이 존재합니다.
원소 속성은 총 4종류로, fire, grass, water, none으로 구성되어 있습니다. 각각의 몬스터 또한 고유의 원소 속성을 가지고 있습니다.
몬스터의 원소 속성에 강점을 보이는 스킬은 데이지가 더 세고, 약점을 보이는 스킬은 더 약한 데미지를 줍니다.

### Result Scene
![스크린샷 2024-01-17 20 09 45](https://github.com/sunohkim/Madcamp_Week3_RPG/assets/37200748/f0b2a4ce-ae1e-4160-b679-2f5f7af9bc3d)

전투에서 패배하면 처음부터 다시 시작해야 합니다.
보물상자를 획득해서 이 게임에서 승리하세요!
