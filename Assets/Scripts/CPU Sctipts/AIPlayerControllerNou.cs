/*using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerBaseNou))]
public class CpuTrainingDummyController : MonoBehaviour
{
    private PlayerBaseNou self;

    [Header("Orientare & Mișcare")]
    public float tolerantaSnap = 45f;      // ± pentru 0/90/180/270
    public float pragLateral = 0.20f;      // |x local| peste care pas lateral
    public float spatiuZMin = 0.20f;       // tampon minim pe Z local
    public float pragZ = 0.30f;            // corecție față/spate
    public float cdPas = 0.20f;            // cooldown pași
    public float cdPivot = 0.25f;          // cooldown pivot

    [Header("Atac")]
    public float razaAtac = 0.75f;         // dist. XZ de la care atacă
    public float tAtac = 1.0f;             // la cât timp rulează zarul
    [Range(0, 1)] public float pAtac = 0.36f;
    public bool comboJabCross = true;
    [Range(0, 1)] public float pCombo = 0.25f;
    public float delayCombo = 0.30f;

    [Header("Block")]
    public float razaBloc = 1.2f;
    public float tBloc = 0.35f;
    [Range(0, 1)] public float pBloc = 0.20f;

    // Timere
    float timPas, timPivot, timAtac, timBloc;

    void Awake() => self = GetComponent<PlayerBaseNou>();

    void Update()
    {
        if (self == null || self.opponent == null) return;
        if (!self.isIdle() || self.IsHit() || self.IsBlocking()) return;

        timPas -= Time.deltaTime;
        timPivot -= Time.deltaTime;
        timAtac -= Time.deltaTime;
        timBloc -= Time.deltaTime;

        // Geometrie
        Vector3 local = transform.InverseTransformPoint(self.opponent.transform.position); local.y = 0f;
        Vector3 delta = self.opponent.transform.position - transform.position; delta.y = 0f;
        float dist = delta.magnitude;

        // 1) Block reactiv (dacă adversarul lovește)
        if (self.opponent.IsPunching() && dist <= razaBloc) IncearcaBlockReactiv();

        // 2) În range: pivot -> posibil atac (nu mai pășește)
        if (dist <= razaAtac)
        {
            if (IncearcaRotireSpreOponent()) return;

            if (timAtac <= 0f)
            {
                timAtac = tAtac;
                if (UnityEngine.Random.value < pAtac) { if (IncearcaAtacRandom()) return; }
            }
            return;
        }

        // 3) În afara range-ului: pivot -> pași (lateral întâi)
        if (IncearcaRotireSpreOponent()) return;

        if (timPas <= 0f && Mathf.Abs(local.x) > pragLateral)
        { if (local.x > 0f) PasDreapta(); else PasStanga(); return; }

        if (timPas <= 0f && Mathf.Abs(local.z) < spatiuZMin && Mathf.Abs(local.x) < pragLateral)
        { PasInapoi(); return; }

        if (timPas <= 0f && local.z > pragZ) { PasInainte(); return; }
        if (timPas <= 0f && local.z < -pragZ) { PasInapoi(); return; }
    }

    // ---------- Pivot ----------
    bool IncearcaRotireSpreOponent()
    {
        if (timPivot > 0f) return false;

        int me = SnapCardinal(transform.eulerAngles.y);
        int opp = SnapCardinal(self.opponent.transform.eulerAngles.y);
        int tgt = (opp + 2) % 4;                 // la 180° de oponent
        if (me == tgt) return false;

        int diff = (tgt - me + 4) % 4;           // 1=stg, 3=dr, 2=180
        bool ok = false;
        if (diff == 1) ok = self.TryPlayPivot(self.GetPivotLeft(), self.GetPivotStamina());
        else if (diff == 3) ok = self.TryPlayPivot(self.GetPivotRight(), self.GetPivotStamina());
        else      ok = self.TryPlayPivot(self.GetPivotLeft(), self.GetPivotStamina());

        if (ok) timPivot = cdPivot;
        return ok;
    }

    int SnapCardinal(float yaw)
    {
        float n = yaw % 360f; if (n < 0f) n += 360f;
        float s = Mathf.Round(n / 90f) * 90f;   // 0/90/180/270
        return (Mathf.RoundToInt(s / 90f)) % 4; // 0..3
    }

    // ---------- Atac ----------
    bool IncearcaAtacRandom()
    {
        if (self.IsHit() || self.IsBlocking()) return false;
        int pick = UnityEngine.Random.Range(0, 6); // 0..5

        switch (pick)
        {
            case 0:
                { // Jab L
                    bool ok = self.TryPlayJab(self.GetJabLeft(), self.GetJabStamina());
                    if (ok && comboJabCross && UnityEngine.Random.value < pCombo)
                        StartCoroutine(JabCross(true));
                    return ok;
                }
            case 1:
                { // Jab R
                    bool ok = self.TryPlayJab(self.GetJabRight(), self.GetJabStamina());
                    if (ok && comboJabCross && UnityEngine.Random.value < pCombo)
                        StartCoroutine(JabCross(false));
                    return ok;
                }
            case 2: return self.TryPlayCross(self.GetCrossLeft(), self.GetCrossStamina());
            case 3: return self.TryPlayCross(self.GetCrossRight(), self.GetCrossStamina());
            case 4: return self.TryPlayUppercut(self.GetUppercutLeft(), self.GetUppercutStamina());
            case 5: return self.TryPlayUppercut(self.GetUppercutRight(), self.GetUppercutStamina());
        }
        return false;
    }

    IEnumerator JabCross(bool jabStanga)
    {
        yield return new WaitForSeconds(delayCombo);
        if (jabStanga) self.TryPlayCross(self.GetCrossLeft(), self.GetCrossStamina());
        else self.TryPlayCross(self.GetCrossRight(), self.GetCrossStamina());
    }

    // ---------- Block ----------
    void IncearcaBlockReactiv()
    {
        if (timBloc > 0f || self.IsBlocking() || self.IsHit() || self.IsPunching()) return;
        timBloc = tBloc;
        if (UnityEngine.Random.value >= pBloc) return;

        string tip = self.opponent.GetCurrentAttackType();
        if (string.IsNullOrEmpty(tip)) return;

        string trig = null;
        if (tip.Contains("Cross Right")) trig = self.GetBlockRight();
        else if (tip.Contains("Cross Left")) trig = self.GetBlockLeft();
        else trig = self.GetBlockCenter(); // Jab/Uppercut

        if (!string.IsNullOrEmpty(trig)) self.TryPlayAction(trig, self.GetBlockStamina());
    }

    // ---------- Pași (root motion) ----------
    void PasStanga() { if (self.TryPlayMove(self.GetStepLeft(), self.GetStepStamina())) timPas = cdPas; }
    void PasDreapta() { if (self.TryPlayMove(self.GetStepRight(), self.GetStepStamina())) timPas = cdPas; }
    void PasInainte() { if (self.TryPlayMove(self.GetStepForward(), self.GetStepStamina())) timPas = cdPas; }
    void PasInapoi() { if (self.TryPlayMove(self.GetStepBack(), self.GetStepStamina())) timPas = cdPas; }
}
*/
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerBaseNou))]
public class AIPlayerControllerNou : MonoBehaviour
{
    private PlayerBaseNou _player;

    [Header("Orientation & Movement")]
    public float CardinalSnapToleranceDegrees = 45f;     // ± snap to 0/90/180/270 (currently unused)
    public float LateralStepThresholdLocalX = 0.20f;     // |local X| above which AI makes a lateral step
    public float MinimumSafeSpaceAlongLocalZ = 0.20f;    // forward/back buffer on local Z
    public float ForwardBackwardCorrectionThreshold = 0.30f; // when to correct forward/backward position
    public float StepCooldownSeconds = 0.20f;            // cooldown for steps (root motion)
    public float PivotCooldownSeconds = 0.25f;           // cooldown for pivots

    [Header("Attack")]
    public float AttackRangeOnGround = 0.75f;            // distance on XZ plane to allow attacking
    public float AttackDecisionIntervalSeconds = 1.0f;   // how often to reconsider attacking
    [Range(0, 1)] public float AttackProbability = 0.36f;
    public bool EnableJabCrossCombo = true;
    [Range(0, 1)] public float ComboTriggerProbability = 0.25f;
    public float ComboStrikePauseSeconds = 0.30f;

    [Header("Defense – Block")]
    public float BlockRangeOnGround = 1.2f;              // max distance to attempt a reactive block
    public float BlockReactionDelaySeconds = 0.35f;      // small delay so blocks aren't instant
    [Range(0, 1)] public float BlockProbability = 0.20f;

    // Internal timers (seconds left until the next allowed action)
    private float timeUntilNextStep;
    private float timeUntilNextPivot;
    private float timeUntilNextAttackDecision;
    private float timeUntilNextBlockAttempt;

    private void Awake() => _player = GetComponent<PlayerBaseNou>();

    private void Update()
    {
        if (_player == null || _player.opponent == null) return;
        if (!_player.isIdle() || _player.IsHit() || _player.IsBlocking()) return;

        // tick timers
        timeUntilNextStep -= Time.deltaTime;
        timeUntilNextPivot -= Time.deltaTime;
        timeUntilNextAttackDecision -= Time.deltaTime;
        timeUntilNextBlockAttempt -= Time.deltaTime;

        Vector3 opponentLocalOnPlane = transform.InverseTransformPoint(_player.opponent.transform.position); opponentLocalOnPlane.y = 0f;
        Vector3 opponentDeltaOnPlane = _player.opponent.transform.position - transform.position; opponentDeltaOnPlane.y = 0f;
        float distanceOnPlane = opponentDeltaOnPlane.magnitude;

        // 1) Reactive block if opponent is punching
        if (_player.opponent.IsPunching() && distanceOnPlane <= BlockRangeOnGround)
            TryReactiveBlock();

        // 2) If in attack range: pivot -> maybe attack (no steps)
        if (distanceOnPlane <= AttackRangeOnGround)
        {
            if (TryFaceOpponentByPivot()) return;

            if (timeUntilNextAttackDecision <= 0f)
            {
                timeUntilNextAttackDecision = AttackDecisionIntervalSeconds;
                if (UnityEngine.Random.value < AttackProbability)
                {
                    if (TryRandomAttack()) return;
                }
            }
            return;
        }

        // 3) Outside attack range: pivot -> steps (prefer lateral)
        if (TryFaceOpponentByPivot()) return;

        if (timeUntilNextStep <= 0f && Mathf.Abs(opponentLocalOnPlane.x) > LateralStepThresholdLocalX)
        { if (opponentLocalOnPlane.x > 0f) StepRight(); else StepLeft(); return; }

        if (timeUntilNextStep <= 0f && Mathf.Abs(opponentLocalOnPlane.z) < MinimumSafeSpaceAlongLocalZ
                                     && Mathf.Abs(opponentLocalOnPlane.x) < LateralStepThresholdLocalX)
        { StepBack(); return; }

        if (timeUntilNextStep <= 0f && opponentLocalOnPlane.z > ForwardBackwardCorrectionThreshold) { StepForward(); return; }
        if (timeUntilNextStep <= 0f && opponentLocalOnPlane.z < -ForwardBackwardCorrectionThreshold) { StepBack(); return; }
    }

    // ---------- Pivot / Facing ----------
    private bool TryFaceOpponentByPivot()
    {
        if (timeUntilNextPivot > 0f) return false;

        int myCardinal = GetCardinalIndex(transform.eulerAngles.y);
        int opponentCardinal = GetCardinalIndex(_player.opponent.transform.eulerAngles.y);
        int targetCardinal = (opponentCardinal + 2) % 4;   
        if (myCardinal == targetCardinal) return false;

        int diff = (targetCardinal - myCardinal + 4) % 4; 
        bool played = false;
        if (diff == 1) played = _player.TryPlayPivot(_player.GetPivotLeft(), _player.GetPivotStamina());
        else if (diff == 3) played = _player.TryPlayPivot(_player.GetPivotRight(), _player.GetPivotStamina());
            else  played = _player.TryPlayPivot(_player.GetPivotLeft(), _player.GetPivotStamina());

        if (played) timeUntilNextPivot = PivotCooldownSeconds;
        return played;
    }

    private int GetCardinalIndex(float yawDegrees)
    {
        float n = yawDegrees % 360f; if (n < 0f) n += 360f;
        float snapped = Mathf.Round(n / 90f) * 90f;    
        return (Mathf.RoundToInt(snapped / 90f)) % 4;  
    }

    // ---------- Attack ----------
    private bool TryRandomAttack()
    {
        if (_player.IsHit() || _player.IsBlocking()) return false;
        int choice = UnityEngine.Random.Range(0, 6); 

        switch (choice)
        {
            case 0: return _player.TryPlayJab(_player.GetJabLeft(), _player.GetJabStamina());
            case 1: return _player.TryPlayJab(_player.GetJabRight(), _player.GetJabStamina());
            case 2: return _player.TryPlayCross(_player.GetCrossLeft(), _player.GetCrossStamina());
            case 3: return _player.TryPlayCross(_player.GetCrossRight(), _player.GetCrossStamina());
            case 4: return _player.TryPlayUppercut(_player.GetUppercutLeft(), _player.GetUppercutStamina());
            case 5: return _player.TryPlayUppercut(_player.GetUppercutRight(), _player.GetUppercutStamina());
        }
        return false;
    }

    // ---------- Defense / Block ----------
    private void TryReactiveBlock()
    {
        if (timeUntilNextBlockAttempt > 0f || _player.IsBlocking() || _player.IsHit() || _player.IsPunching()) return;
        timeUntilNextBlockAttempt = BlockReactionDelaySeconds;
        if (UnityEngine.Random.value >= BlockProbability) return;

        string attackType = _player.opponent.GetCurrentAttackType();
        if (string.IsNullOrEmpty(attackType)) return;

        string actionTrigger;
        if (attackType.Contains("Cross Right")) actionTrigger = _player.GetBlockRight();
        else if (attackType.Contains("Cross Left")) actionTrigger = _player.GetBlockLeft();
        else actionTrigger = _player.GetBlockCenter(); 

        if (!string.IsNullOrEmpty(actionTrigger))
            _player.TryPlayAction(actionTrigger, _player.GetBlockStamina());
    }

    // ---------- Steps (root motion) ----------
    private void StepLeft() { if (_player.TryPlayMove(_player.GetStepLeft(), _player.GetStepStamina())) timeUntilNextStep = StepCooldownSeconds; }
    private void StepRight() { if (_player.TryPlayMove(_player.GetStepRight(), _player.GetStepStamina())) timeUntilNextStep = StepCooldownSeconds; }
    private void StepForward() { if (_player.TryPlayMove(_player.GetStepForward(), _player.GetStepStamina())) timeUntilNextStep = StepCooldownSeconds; }
    private void StepBack() { if (_player.TryPlayMove(_player.GetStepBack(), _player.GetStepStamina())) timeUntilNextStep = StepCooldownSeconds; }
}
