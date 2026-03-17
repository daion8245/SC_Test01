# Unity 프로젝트 구조화 기획서
> 2026 지방 기능경기대회 | 탑뷰 불렛헬 슈터 | 16시간 제한

---

## 섹션 1: 폴더/파일 구조도

### 현재 구조

```
Assets/
├── Script/                         ← 모든 스크립트가 평탄하게 나열
│   ├── Character.cs                (스탯+충돌+사망 혼합)
│   ├── Player.cs
│   ├── PlayerMove.cs
│   ├── GameManager.cs
│   ├── UiManager.cs
│   ├── IEnemies.cs                 (미사용 인터페이스)
│   ├── IBullets.cs                 (미사용 인터페이스)
│   ├── RsacEnemy.cs                (베이스 겸 실체 클래스)
│   ├── HvyaEnemy.cs
│   ├── TrsTEnemy.cs
│   ├── InterceptorEnemy.cs
│   ├── StraightBullet.cs
│   ├── GuidedMissile.cs            (ShotgunHomingBullet과 중복 로직)
│   ├── ShotgunHomingBullet.cs      (GuidedMissile과 중복 로직)
│   └── Shelling.cs                 (무한 while 루프 버그)
├── Prefab/
│   ├── GameManager.prefab
│   ├── Player.prefab
│   ├── RSAC_Emey.prefab
│   ├── Havy_Emey.prefab
│   ├── TRST_Emey.prefab
│   ├── Interceptor_Emey.prefab
│   ├── StraightBullet.prefab
│   ├── Guided_Missile.prefab
│   └── shotgunBulet.prefab
├── Scenes/
│   ├── Main game.unity
│   └── SampleScene.unity
├── Material/
└── Settings/
```

### 제안 구조

```
Assets/
├── Scripts/
│   ├── Core/
│   │   ├── GameManager.cs          (기존 — 필드 정리)
│   │   ├── StageManager.cs         (신규)
│   │   └── SaveManager.cs          (신규)
│   ├── Character/
│   │   └── Character.cs            (기존 — 충돌 로직 제거, 스탯만)
│   ├── Player/
│   │   ├── Player.cs               (기존 — 충돌 처리 이동)
│   │   ├── PlayerMove.cs           (기존 유지)
│   │   └── BombSystem.cs           (신규)
│   ├── Enemy/
│   │   ├── EnemyBase.cs            (기존 RsacEnemy 이름변경 + 정리)
│   │   ├── ScoutEnemy.cs           (기존 RsacEnemy 실체 → ScoutEnemy로 분리)
│   │   ├── FighterEnemy.cs         (기존 TrsTEnemy 이름변경)
│   │   ├── HeavyEnemy.cs           (기존 HvyaEnemy 이름변경)
│   │   ├── InterceptorEnemy.cs     (기존 유지)
│   │   ├── BomberEnemy.cs          (신규 — Shelling 사용)
│   │   └── BossEnemy.cs            (신규)
│   ├── Bullet/
│   │   ├── BulletBase.cs           (신규 — 공통 로직 추출)
│   │   ├── StraightBullet.cs       (기존 — BulletBase 상속으로 정리)
│   │   ├── GuidedMissile.cs        (기존 — BulletBase 상속으로 정리)
│   │   ├── ShotgunHomingBullet.cs  (기존 — BulletBase 상속으로 정리)
│   │   └── Shelling.cs             (기존 — 무한루프 버그 수정)
│   ├── Item/
│   │   ├── ItemBase.cs             (신규)
│   │   ├── HpItem.cs               (신규)
│   │   ├── InvincibleItem.cs       (신규)
│   │   └── DefenseItem.cs          (신규)
│   ├── Parts/
│   │   ├── PartsBase.cs            (신규)
│   │   └── ForcedGuidanceParts.cs  (신규 — 강제유도, 최우선 파츠)
│   ├── Obstacle/
│   │   └── Meteor.cs               (신규)
│   └── UI/
│       ├── UiManager.cs            (기존 — 확장)
│       ├── ShopUI.cs               (신규)
│       └── RankingUI.cs            (신규)
├── Prefab/                         (이름 오타 수정 권장)
├── Scenes/
│   ├── MainMenu.unity              (신규)
│   ├── Stage1.unity                (신규)
│   ├── Stage2.unity                (신규)
│   ├── Stage3.unity                (신규)
│   └── (기존 씬 유지 가능)
├── Material/
└── Settings/
```

> **파일 수**: 현재 15개 → 제안 27개 (신규 기능 12개 포함, 순수 리팩토링으로 증가한 파일은 2개뿐)

---

## 섹션 2: 클래스 다이어그램 (텍스트)

```
[MonoBehaviour]
    │
    ├──▶ [Character]                   (스탯: hp, maxHp, atk, isAlive)
    │       │
    │       ├──▶ [Player]              (충돌→Enemy 밀기, CrashBullets 오버라이드)
    │       │       └── has → [PlayerMove]   (이동/회전 물리)
    │       │
    │       └──▶ [EnemyBase]           (발사 루프, LookAt 플레이어)
    │               ├── implements → [IEnemies] (※삭제 검토)
    │               ├──▶ [ScoutEnemy]        (단발, 직선탄)
    │               ├──▶ [FighterEnemy]      (3연발, 유도탄)
    │               ├──▶ [HeavyEnemy]        (산탄 N발)
    │               ├──▶ [InterceptorEnemy]  (조준선 표시 후 직선탄)
    │               ├──▶ [BomberEnemy]       (Shelling 사용)
    │               └──▶ [BossEnemy]         (다중 패턴, 몸통박치기만 피격)
    │
    ├──▶ [BulletBase]                  (Damage, BulletSpeed, 거리초과 파괴)
    │       ├── implements → [IBullets] (※삭제 검토)
    │       ├──▶ [StraightBullet]      (직선 이동)
    │       ├──▶ [GuidedMissile]       (일정 시간 유도 후 직선)
    │       ├──▶ [ShotgunHomingBullet] (산탄→정지→유도)
    │       └──▶ [Shelling]            (플레이어 위치 추적→범위 폭발)
    │
    ├──▶ [GameManager]                 (Singleton, 플레이어 위치·참조 보관)
    │       └── has → [StageManager]   (스테이지 흐름 제어)
    │
    ├──▶ [UiManager]                   (Singleton, HP바·점수·폭탄 UI)
    ├──▶ [ShopUI]                      (파츠 구매 UI)
    ├──▶ [RankingUI]                   (랭킹 표시 UI)
    ├──▶ [SaveManager]                 (PlayerPrefs 기반 세이브/로드)
    ├──▶ [BombSystem]                  (폭탄 발동, 화면 내 탄막 전체 파괴)
    ├──▶ [ItemBase]  ──▶ [HpItem], [InvincibleItem], [DefenseItem]
    ├──▶ [PartsBase] ──▶ [ForcedGuidanceParts], ... (퀵슬롯 2개)
    └──▶ [Meteor]                      (장애물, 플레이어 HP 감소)

범례:
──▶  상속 (extends)
─ ─▶ 구현 (implements)
has  컴포넌트 참조 또는 필드
```

---

## 섹션 3: 리팩토링 체크리스트

> **규칙**: 이 섹션은 기존 코드 정리만 포함. 신규 기능 구현은 섹션 4로.

---

### [CRITICAL] 버그 수정

- [ ] **Shelling.cs** — `FollowingPlayer()` 무한 while 루프 제거, Update/Coroutine으로 교체
  - 이유: `while (_loop)` 루프가 메인 스레드를 영구 점유 → 게임 완전 프리즈
  - 위험도: **높음** (현재 게임 실행 불가 수준)
  - 예상 소요: ~15분

---

### 성능 개선

- [ ] **RsacEnemy.cs (→ EnemyBase.cs)** — `Update()`에서 매 프레임 `StartCoroutine()` 방지
  - 이유: `loop = true` 직후 다음 프레임에 즉시 재호출될 수 있어 코루틴이 중복 실행될 위험
  - 수정: `Update()` 대신 `Start()`에서 `StartCoroutine(FireLoop())`로 단일 루프 코루틴화
  - 위험도: **중간**
  - 예상 소요: ~20분

- [ ] **UiManager.cs** — `Update()`에서 매 프레임 HP바 갱신 → 이벤트 or `OnHpChanged` 콜백으로 변경
  - 이유: 불필요한 연산. HP가 변할 때만 갱신하면 됨
  - 수정: `Character.Hp` setter에서 `onHpChanged?.Invoke()` 호출, UiManager가 구독
  - 위험도: **낮음**
  - 예상 소요: ~20분

---

### 중복 코드 제거

- [ ] **BulletBase.cs 신규 생성** — `GuidedMissile`·`ShotgunHomingBullet`·`StraightBullet` 공통 로직 추출
  - 이유: 세 클래스 모두 `Damage/BulletSpeed` 초기화, `ApplyDamage`, `거리초과 Destroy`, `OnTriggerEnter→ApplyDamage` 패턴이 완전히 동일
  - 추출 내용: `Damage`, `BulletSpeed` 프로퍼티 + `ApplyDamage(Character)` + `_maxDistanceSqr` + `OnTriggerEnter`
  - 위험도: **중간** (프리팹 컴포넌트 재연결 필요)
  - 예상 소요: ~30분

- [ ] **GuidedMissile.cs** — `BulletBase` 상속으로 중복 필드/메서드 제거
  - 이유: `ApplyDamage`, `OnTriggerEnter`, `_maxDistanceSqr` 계산이 StraightBullet과 동일
  - 위험도: **낮음**
  - 예상 소요: ~15분

- [ ] **ShotgunHomingBullet.cs** — `BulletBase` 상속으로 중복 필드/메서드 제거
  - 이유: 위와 동일
  - 위험도: **낮음**
  - 예상 소요: ~15분

---

### 인터페이스 정리

- [ ] **IEnemies.cs** — 삭제 또는 실제 다형성 활용 코드 추가
  - 이유: `GameManager.enemies` 리스트가 선언만 되어 있고, `IEnemies.FiringBullet()`을 인터페이스를 통해 호출하는 코드가 전혀 없음. `FiringBullet()`은 `RsacEnemy`의 `public` 메서드로도 직접 접근 가능
  - 수정: BombSystem(섹션4)에서 `enemies` 리스트를 실제로 순회할 예정이면 유지, 아니면 삭제
  - 위험도: **낮음**
  - 예상 소요: ~10분

- [ ] **IBullets.cs** — 삭제 또는 BombSystem에서 실제 활용
  - 이유: `GameManager.bullets` 리스트에 `GuidedMissile`/`ShotgunHomingBullet`이 Add/Remove하지만, 이 리스트를 실제로 읽는 코드가 없음
  - 수정: 폭탄 기능(섹션4)에서 탄막 전체 파괴 시 이 리스트를 활용하면 즉시 유의미해짐 → **삭제보다 활용 권장**
  - 위험도: **낮음**
  - 예상 소요: ~10분

---

### 클래스 이름/구조 정리

- [ ] **RsacEnemy.cs → EnemyBase.cs** — 클래스명 변경 및 직접 사용 불가로 `abstract` 처리
  - 이유: `RsacEnemy`라는 이름이 의미 불명확. 현재 자체 프리팹(RSAC_Emey.prefab)이 있어 베이스 겸 실체 클래스로 혼용 중. 정찰기(`ScoutEnemy`)를 별도로 분리해야 함
  - 수정: `EnemyBase`를 `abstract class`로 변경, 기존 `RsacEnemy` 동작은 `ScoutEnemy`로 분리
  - 위험도: **중간** (RSAC_Emey 프리팹의 스크립트 컴포넌트 재연결 필요)
  - 예상 소요: ~25분

- [ ] **HvyaEnemy.cs → HeavyEnemy.cs**, **TrsTEnemy.cs → FighterEnemy.cs** — 이름 변경
  - 이유: 의미 불명확한 이름. 프리팹명과 통일
  - 위험도: **낮음** (프리팹 컴포넌트 이름만 업데이트)
  - 예상 소요: ~10분

- [ ] **Character.cs** — `CharacterType` enum과 충돌 처리 분리
  - 이유: 스탯 클래스에 충돌 처리 로직(`CrashTest`, `CrashEntity`, `CrashBullets`)이 섞여 있음. 충돌은 Player/Enemy 각자에서 처리하는 것이 자연스러움
  - 수정: `CrashTest`/`CrashEntity`/`CrashBullets`를 `protected virtual`로 유지하되, `Character`에서는 기본 구현만, 실제 로직은 `Player`와 `EnemyBase` 오버라이드로 이동
  - 위험도: **낮음** (이미 virtual/override 구조)
  - 예상 소요: ~15분

---

**리팩토링 합계: 약 2시간 45분**

---

## 섹션 4: 신규 구현 TODO 리스트

> **규칙**: 기획서 PDF 요구사항 기반, 미구현 기능만. 리팩토링 내용 제외.

---

### 핵심 게임 시스템

- [ ] **BulletBase 추상 클래스** — 탄막 공통 베이스 (섹션3 리팩토링과 연계)
  - 관련 스크립트: `Bullet/BulletBase.cs`
  - 우선순위: **필수**
  - 예상 소요: ~20분

- [ ] **BombSystem** — 폭탄 발동 시 화면 내 모든 탄막(`IBullets` 리스트) 즉시 파괴
  - 관련 스크립트: `Player/BombSystem.cs`
  - 기획 근거: "폭탄 사용 시 화면에 보이는 모든 탄막이 제거된다 (적은 제거되지 않는다)"
  - 우선순위: **필수**
  - 예상 소요: ~30분

- [ ] **ScoutEnemy** — 기존 RsacEnemy 기능 그대로, 단발 직선탄
  - 관련 스크립트: `Enemy/ScoutEnemy.cs`
  - 기획 근거: 5종 적 요구 (현재 4종)
  - 우선순위: **필수**
  - 예상 소요: ~15분

- [ ] **BomberEnemy** — Shelling(곡사 폭탄) 패턴 사용하는 적
  - 관련 스크립트: `Enemy/BomberEnemy.cs`
  - 기획 근거: "폭격기 — 곡사 폭탄, 예측 공격", 5종 적 요구 충족
  - 우선순위: **필수**
  - 예상 소요: ~30분

- [ ] **BossEnemy** — 각 스테이지 보스, 다중 탄막 패턴, 몸통박치기만 피격
  - 관련 스크립트: `Enemy/BossEnemy.cs`
  - 기획 근거: "스테이지별 보스가 등장", "보스는 몸통박치기를 통해서만 피해를 줄 수 있다"
  - 우선순위: **필수**
  - 예상 소요: ~60분

---

### 플레이어 시스템

- [ ] **Player HP 피격 처리 실제 구현** — 현재 `CrashBullets`가 `Debug.Log`만 출력, 실제 데미지 미적용
  - 관련 스크립트: `Player/Player.cs` 수정
  - 기획 근거: "주인공이 적의 공격을 받으면 생명치는 일정 비율로 감소"
  - 우선순위: **필수**
  - 예상 소요: ~15분

- [ ] **GameOver & 스테이지 재시작** — HP 0 시 현재 스테이지 재시작 (파츠·돈 유지)
  - 관련 스크립트: `Player/Player.cs` + `Core/StageManager.cs`
  - 기획 근거: "주인공의 체력이 0이 되면 게임오버, 해당 스테이지를 다시 시작한다"
  - 우선순위: **필수**
  - 예상 소요: ~30분

- [ ] **적 맵 밖 복귀** — 플레이어에게 밀린 적이 맵 경계 밖으로 나가면 안쪽으로 복귀
  - 관련 스크립트: `Enemy/EnemyBase.cs` 수정
  - 기획 근거: "적을 맵 끝까지 밀었을 경우 다시 화면 안으로 들어오게 한다"
  - 우선순위: **필수**
  - 예상 소요: ~20분

---

### 스테이지·게임 흐름

- [ ] **StageManager** — 스테이지 전환, 적 스폰, 클리어 조건 판정
  - 관련 스크립트: `Core/StageManager.cs`
  - 기획 근거: "총 3개의 스테이지", 각 스테이지별 적 종류 증가
  - 우선순위: **필수**
  - 예상 소요: ~60분

- [ ] **씬 구성** — MainMenu, Stage1~3 씬 분리 및 씬 전환 로직
  - 관련 스크립트: `Core/StageManager.cs` + SceneManager 호출
  - 기획 근거: "게임 시작, 구동, 종료 화면"
  - 우선순위: **필수**
  - 예상 소요: ~30분

---

### UI

- [ ] **UiManager 확장** — 점수 표시, 폭탄 사용가능 여부, 스테이지 타이머 추가
  - 관련 스크립트: `UI/UiManager.cs` 수정
  - 기획 근거: "점수 정보", "폭탄 사용 가능 여부", "스테이지 별 시간 흐름 정보"
  - 우선순위: **필수**
  - 예상 소요: ~40분

- [ ] **적 HP 바** — 보스 및 일반적 HP UI 표시
  - 관련 스크립트: `UI/UiManager.cs` + EnemyBase에 HP 변경 이벤트
  - 기획 근거: "적의 HP" UI 항목
  - 우선순위: **필수**
  - 예상 소요: ~30분

- [ ] **메인 메뉴 UI** — 게임 시작, 랭킹, 종료 버튼
  - 관련 스크립트: `UI/UiManager.cs` (MainMenu 씬용)
  - 기획 근거: "메뉴 화면에서는 게임플레이, 랭킹, 종료 등과 같은 메뉴를 선택"
  - 우선순위: **필수**
  - 예상 소요: ~20분

- [ ] **ShopUI** — 파츠 구매·장착 화면, 퀵슬롯 2개 표시
  - 관련 스크립트: `UI/ShopUI.cs`
  - 기획 근거: "각 스테이지 클리어 시 상점을 이용", "총 퀵슬롯의 수는 2개로 제한"
  - 우선순위: **필수**
  - 예상 소요: ~60분

- [ ] **RankingUI** — 내림차순 5명 이상 랭킹 표시 + 이름 입력
  - 관련 스크립트: `UI/RankingUI.cs`
  - 기획 근거: "랭킹(내림차순 정렬) 시스템", "순위, 이름, 점수를 포함하고 5명이상 표현"
  - 우선순위: **필수**
  - 예상 소요: ~40분

---

### 아이템·파츠 시스템

- [ ] **ItemBase + 3종 아이템** — HP회복, 무적(일정 시간), 방어력 강화
  - 관련 스크립트: `Item/ItemBase.cs`, `Item/HpItem.cs`, `Item/InvincibleItem.cs`, `Item/DefenseItem.cs`
  - 기획 근거: "3종 이상의 게임 아이템: 생명력 회복, 일정 시간 무적, 일정 시간 방어력 강화"
  - 우선순위: **필수**
  - 예상 소요: ~50분

- [ ] **PartsBase + 강제유도 파츠** — 퀵슬롯 2개에 장착·발동 시스템
  - 관련 스크립트: `Parts/PartsBase.cs`, `Parts/ForcedGuidanceParts.cs`
  - 기획 근거: "파츠 시스템", "강제 유도 — 화면 상 모든 미사일을 바라보는 적에게 강제 유도"
  - 우선순위: **필수** (파츠 1종만 최소 구현, 나머지는 권장)
  - 예상 소요: ~60분

- [ ] **파츠 추가 구현** — 시간정지, 공격반사, 블랙홀, 감속장 중 선택 구현
  - 관련 스크립트: `Parts/` 하위 각 파일
  - 우선순위: **권장** (강제유도 완성 후)
  - 예상 소요: ~각 30분

- [ ] **돈 시스템** — 적 처치 시 돈 획득, StageManager 연동
  - 관련 스크립트: `Core/GameManager.cs` 또는 `Core/StageManager.cs`
  - 기획 근거: "스테이지를 클리어 했을 시 제거한 적의 수에 비례하여 돈을 획득"
  - 우선순위: **필수**
  - 예상 소요: ~20분

---

### 세이브/로드

- [ ] **SaveManager** — PlayerPrefs 기반, 보유 돈·파츠·아이템·현재 스테이지·퀵슬롯 저장
  - 관련 스크립트: `Core/SaveManager.cs`
  - 기획 근거: "저장되는 데이터: 보유 돈, 보유 파츠, 보유 아이템, 현재 스테이지, 장착된 파츠 정보, 퀵슬롯"
  - 우선순위: **필수**
  - 예상 소요: ~40분

---

### 장애물 & 기타

- [ ] **Meteor (운석)** — 랜덤 이동, 플레이어 HP 감소, 스테이지 2~3에 배치
  - 관련 스크립트: `Obstacle/Meteor.cs`
  - 기획 근거: "운석 등(플레이어에게 타격을 주는 장애물)"
  - 우선순위: **필수**
  - 예상 소요: ~30분

- [ ] **치트키** — F1-F12, 디버그모드 / 무적 / 강제 적 사망 / 돈 추가
  - 관련 스크립트: `Core/GameManager.cs` 또는 별도 `CheatManager.cs`
  - 기획 근거: "치트키 상세 설계" (디버그모드, 무적, 강제적사망, 돈추가)
  - 우선순위: **필수** (채점 시연에 필요)
  - 예상 소요: ~20분

- [ ] **사운드** — 배경음 + 효과음(총알 발사, 피격, 아이템 획득)
  - 관련 스크립트: `Core/GameManager.cs`에 AudioSource 추가 또는 SoundManager
  - 기획 근거: "효과음과 배경음을 게임 내에서 적용"
  - 우선순위: **권장**
  - 예상 소요: ~30분

---

**신규 구현 합계 (필수만): 약 9시간**
**리팩토링 + 신규 필수 합계: 약 11시간 45분 (버퍼 4시간 15분)**

---

## 섹션 5: 변경 이유 상세 설명

### 리팩토링 근거

#### `BulletBase.cs` 추출
`GuidedMissile`, `ShotgunHomingBullet`, `StraightBullet` 세 클래스는 아래 코드를 **정확히 동일하게** 복사하고 있다:
```csharp
// 세 클래스 모두 동일
public void ApplyDamage(Character character) {
    character.Hp -= Damage;
    Destroy(gameObject);
}
private void OnTriggerEnter(Collider other) {
    if (other.TryGetComponent(out Character character))
        ApplyDamage(character);
}
```
또한 `[SerializeField] private int damage` → `Damage = damage` 초기화 패턴, `_maxDistanceSqr = maxDistance * maxDistance` 계산, 거리 초과 `Destroy` 로직도 반복된다. `BulletBase : MonoBehaviour, IBullets`로 추출하면 신규 탄막 추가 시 작성 코드가 절반 이하로 줄어든다.

#### `EnemyBase` + `ScoutEnemy` 분리
`RsacEnemy`가 베이스 클래스이면서 동시에 `RSAC_Emey.prefab`으로 씬에 직접 배치되는 이중 역할을 한다. 이 상태에서 베이스 클래스를 수정하면 정찰기 동작에 의도치 않은 영향을 준다. `EnemyBase`를 `abstract`로 만들면 컴파일 타임에 직접 인스턴스화를 방지할 수 있다. `ScoutEnemy`는 기존 `RsacEnemy`의 Fire() 동작을 그대로 가져가므로 코드 변경이 거의 없다.

#### `IEnemies` / `IBullets` 활용 전환
두 인터페이스 모두 `GameManager`에 리스트가 있으나 실제로 리스트를 **읽는** 코드가 없었다. `BombSystem`에서 `GameManager.bullets`를 순회해 탄막을 전부 파괴하도록 구현하면 `IBullets`가 즉시 유의미해진다. `IEnemies.FiringBullet()`은 치트키 "강제 적 사망" 기능에서 `GameManager.enemies`를 순회해 한꺼번에 처리하는 데 활용 가능하다.

#### `UiManager` 이벤트 기반 전환
Unity에서 `Update()`는 60fps 기준 초당 60회 호출된다. HP는 피격 시에만 변한다. 이벤트 기반으로 변경하면 불필요한 `InverseLerp` 연산을 제거할 수 있다. `System.Action onHpChanged`를 `Character.Hp` setter에 추가하는 것으로 최소한의 변경으로 달성 가능하다.

#### 적 맵 밖 복귀 구현 위치
기획서 규칙 ④에서 "맵 끝까지 밀었을 경우 다시 화면 안으로 들어오게 한다"고 명시. `EnemyBase.LateUpdate()`에서 이미 매 프레임 위치를 확인하므로, 이곳에 맵 경계 체크를 추가하는 것이 가장 비용이 낮다. 별도 컴포넌트 불필요.

---

### 신규 구현 설계 근거

#### StageManager 설계
`GameManager`에 스테이지 로직을 추가하면 God Object가 된다. `StageManager`를 별도 컴포넌트로 분리해 스테이지 전환, 적 스폰 타이밍, 클리어 조건(적 전멸 또는 보스 처치)을 전담한다. `GameManager`는 전역 상태(플레이어 위치, 돈, 점수)만 보유하는 순수 데이터 허브로 유지한다.

#### SaveManager — PlayerPrefs 선택 이유
16시간 제한 내에서 파일 I/O보다 `PlayerPrefs`가 훨씬 빠르게 구현 가능하다. 기획서 저장 데이터(돈, 파츠, 아이템, 스테이지, 퀵슬롯)는 모두 int/string으로 직렬화 가능하다. JSON 직렬화가 필요하면 `JsonUtility`를 `PlayerPrefs`와 조합하는 방식을 사용한다.

#### BombSystem 분리 이유
폭탄은 PlayerMove, Player 스탯과 독립적인 액티브 스킬이다. Player.cs에 직접 넣으면 책임이 섞인다. `BombSystem` 컴포넌트를 Player 프리팹에 추가하는 방식으로 분리하면 폭탄 쿨타임, 보유 개수를 독립적으로 조정 가능하다.

#### ItemBase 상속 구조 선택
3종 아이템(HP회복, 무적, 방어력강화)은 "획득 즉시 효과 적용 → 일정 시간 후 소멸" 패턴이 동일하다. `ItemBase`에서 `OnCollect(Player player)` 추상 메서드와 `duration`, `OnTriggerEnter` 처리를 공통화하면 각 아이템 클래스는 `OnCollect` 오버라이드만 작성하면 된다.

#### 파츠 시스템 — 최소 1종 우선 구현 전략
기획서는 5종 파츠를 제시하지만 퀵슬롯이 2개뿐이다. 채점에서 파츠 **시스템(구조)**이 동작하는지를 보므로, 강제유도 1종을 완전히 구현한 뒤 시간이 남으면 추가하는 전략이 현실적이다. `PartsBase`의 `Activate()` 추상 메서드만 오버라이드하면 되도록 구조화해 추가 비용을 최소화한다.

#### 치트키 구현 위치
`GameManager`에 `Update()`에서 Function 키 입력을 감지하는 코드를 추가하는 것이 가장 단순하다. 별도 Manager 파일을 만들 필요 없이 기존 싱글톤에서 처리하면 된다.

---

## 섹션 6: 심각한 성능 저하 코드 리팩토링

### 6-1. [CRITICAL] Shelling.cs — 무한 while 루프로 게임 프리즈

#### 문제 코드
```csharp
// Shelling.cs:40-46 — 현재 코드 (버그)
private void FollowingPlayer()
{
    while (_loop)   // ← yield 없이 메인 스레드를 무한 점유
    {
        transform.position = GameManager.Instance.playerPosition;
    }
}
```
`StartCoroutine(ShellingStart())`에서 `FollowingPlayer()`를 호출하는데, 이 함수는 `yield return` 없는 무한 while 루프다. Unity 코루틴도 메인 스레드에서 실행되므로, 이 루프에 진입하는 순간 **게임이 완전히 멈춘다(무한 루프, 응답 없음)**.

#### 수정 코드
```csharp
// Shelling.cs — 수정 후
private IEnumerator ShellingStart()
{
    float elapsed = 0f;
    while (elapsed < inductiveTime)
    {
        transform.position = GameManager.Instance.playerPosition;
        elapsed += Time.deltaTime;
        yield return null;   // ← 매 프레임 한 번씩 실행, 메인 스레드 반환
    }
    yield return new WaitForSeconds(2f);
    foreach (var character in _charactersInTrigger)
        character.Hp -= Damage;
    Destroy(gameObject);
}
// FollowingPlayer() 메서드 삭제
```

#### 선택 근거
- `yield return null`은 다음 프레임까지 실행을 양보하므로 메인 스레드가 블록되지 않음
- `while (_loop)` + 외부에서 `_loop = false` 패턴 대신, `elapsed < inductiveTime`으로 루프 탈출 조건을 명확히 함
- 코루틴 내부에서 직접 위치 추적 → 별도 `FollowingPlayer()` 메서드 불필요

---

### 6-2. [HIGH] RsacEnemy.cs — 매 프레임 StartCoroutine 중복 호출

#### 문제 코드
```csharp
// RsacEnemy.cs:12-18 — 현재 코드
protected void Update()
{
    if (loop)
    {
        StartCoroutine(Fire());   // ← 매 프레임 새 코루틴 시작
    }
}
```
`loop = true`인 첫 프레임에 `StartCoroutine`이 호출된다. `Fire()` 코루틴 안에서 `loop = false`로 설정하지만, **같은 프레임의 Update가 이미 수행되었거나**, 코루틴 종료 후 `loop = true`로 돌아온 직후 다음 `Update`에서 다시 코루틴이 중복 시작될 위험이 있다. 실제로 `HvyaEnemy`처럼 여러 프레임에 걸쳐 작동하는 서브클래스에서 타이밍 버그가 발생할 수 있다.

#### 수정 코드
```csharp
// EnemyBase.cs — 수정 후
private void Start()
{
    if (GameManager.Instance != null)
        GameManager.Instance.enemies.Add(this);
    StartCoroutine(FireLoop());   // 단일 루프 코루틴 시작
}

// Update() 완전 제거

private IEnumerator FireLoop()
{
    while (isAlive)
    {
        yield return Fire();    // Fire 코루틴이 끝날 때까지 대기 후 다음 사이클
    }
}

protected virtual IEnumerator Fire()
{
    yield return new WaitForSeconds(fireRate);
    FiringBullet();
}
```

#### 선택 근거
- `while (isAlive)` 루프 하나가 `Fire()` 코루틴이 완료될 때까지 기다렸다 재실행 → 중복 시작 원천 차단
- `Update()` 제거로 매 프레임 조건 체크 비용 0
- 서브클래스(`FighterEnemy`, `HeavyEnemy` 등)는 `Fire()` 오버라이드만 유지하면 됨 — 하위 호환성 유지

---

### 6-3. [MEDIUM] UiManager.cs — 매 프레임 불필요한 연산

#### 문제 코드
```csharp
// UiManager.cs:24-27 — 현재 코드
private void Update()
{
    hpBar.value = Mathf.InverseLerp(0f, player.MaxHp, player.Hp); // 60fps마다 실행
}
```

#### 수정 코드
```csharp
// Character.cs — Hp setter에 이벤트 추가
public System.Action onHpChanged;
public int Hp
{
    get => hp;
    set
    {
        hp = Mathf.Clamp(value, 0, maxHp);
        onHpChanged?.Invoke();   // ← HP 변경 시에만 알림
        if (hp <= 0 && isAlive) { isAlive = false; Dead(); }
    }
}

// UiManager.cs — 수정 후
private void Start()
{
    player.onHpChanged += RefreshHpBar;
    RefreshHpBar();   // 초기값 설정
}

private void RefreshHpBar()
{
    hpBar.value = Mathf.InverseLerp(0f, player.MaxHp, player.Hp);
}

// Update() 제거
```

#### 선택 근거
- HP는 피격, 아이템 획득 등 특정 이벤트 시에만 변한다. 매 프레임 60회 연산할 이유가 없음
- `System.Action`은 Unity 이벤트 시스템보다 가볍고 별도 에셋 없이 사용 가능
- `Update()` 제거로 UiManager가 매 프레임 Update 큐에 등록되지 않음

---

### 6-4. [MEDIUM] StraightBullet.cs — 원점 기준 거리 체크 오류

#### 문제 코드
```csharp
// StraightBullet.cs:25-26
if (transform.position.sqrMagnitude > maxDistanceSqr)
    Destroy(gameObject);
```
`transform.position.sqrMagnitude`는 **월드 원점(0,0,0)으로부터의 거리**다. 맵 가장자리나 원점에서 멀리 배치된 적이 발사하면, 발사 즉시 `sqrMagnitude`가 이미 크기 때문에 **탄막이 즉시 사라지거나**, 반대로 원점 근처에서 발사 시 아주 먼 거리까지 날아가는 비대칭 문제가 발생한다.

#### 수정 코드 (BulletBase에 통합)
```csharp
// BulletBase.cs
private Vector3 _spawnPosition;

protected virtual void Start()
{
    _spawnPosition = transform.position;   // 발사 시점 위치 기록
}

protected void CheckMaxDistance()
{
    if ((transform.position - _spawnPosition).sqrMagnitude > maxDistanceSqr)
        Destroy(gameObject);
}
```

#### 선택 근거
- 발사 지점(`_spawnPosition`) 기준 거리로 변경 → 맵 어디에서 발사해도 일관된 사거리 보장
- `BulletBase`에 공통화하면 `GuidedMissile`, `ShotgunHomingBullet`의 동일한 버그도 함께 수정됨

---

**성능 수정 합계 예상 소요: 약 1시간 20분** (섹션 3 리팩토링 시간 내 포함)
