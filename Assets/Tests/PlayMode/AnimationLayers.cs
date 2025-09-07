using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class AnimationLayers : TestGameLoader
{

    TestGameLoader loader;

    PlayerBaseTest player;
    PlayerBaseTest opponent;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        yield return SceneManager.LoadSceneAsync("test");
        yield return null;

        yield return new WaitForSeconds(1f); // Așteaptă un cadru pentru a te asigura că scena este încărcată complet

        loader = GameObject.FindObjectOfType<TestGameLoader>();
        Assert.IsNotNull(loader, "TestGameLoader not found!");

        Assert.IsNotNull(loader.p1, "Player (p1) not initialized!");
        Assert.IsNotNull(loader.op, "Opponent (op) not initialized!");
        player = loader.p1;
        opponent = loader.op;

        yield return new WaitForSeconds(1f); // Așteaptă un cadru pentru a te asigura că scena este încărcată complet

    }


    [UnityTest]

    public IEnumerator tryPlayPunch()
    {
        player.TryPlayPunch(player.GetJabRight());
        yield return new WaitForSeconds(0.3f);
        bool testresult = player.IsPunching();
        yield return new WaitForSeconds(0.1f);
        Assert.IsTrue(testresult);

        yield return new WaitForSeconds(3f);

        loader.op.TryPlayPunch(loader.op.GetJabRight());
        yield return new WaitForSeconds(0.3f);
        bool testresultop = loader.op.IsPunching();
        yield return new WaitForSeconds(0.1f);
        Assert.IsTrue(testresultop);
    }


    [UnityTest]
    public IEnumerator JabRightCritTest()
    {
        
        player.TryPlayPunch(player.GetJabRight());
        opponent.TryPlayPunch(opponent.GetJabRight());
        yield return new WaitForSeconds(0.2f);
        opponent.ReceiveHit(player, "Head", player.lastattack, true);
        yield return new WaitForSeconds(0.3f);
        bool testresult = opponent.IsBigPunchToTheFace();
        yield return new WaitForSeconds(0.5f);
        Assert.IsTrue(testresult);

        yield return new WaitForSeconds(3f);

        player.TryPlayPunch(player.GetJabRight());
        opponent.TryPlayPunch(opponent.GetJabLeft());
        yield return new WaitForSeconds(0.2f);
        opponent.ReceiveHit(player, "Head", player.lastattack, true);
        yield return new WaitForSeconds(0.3f);
        testresult = opponent.IsBigPunchToTheFace();
        yield return new WaitForSeconds(0.5f);
        Assert.IsTrue(testresult);


        yield return new WaitForSeconds(3f);

        player.TryPlayPunch(player.GetJabRight());
        opponent.TryPlayPunch(opponent.GetCrossRight());
        yield return new WaitForSeconds(0.2f);
        opponent.ReceiveHit(player, "Head", player.lastattack, true);
        yield return new WaitForSeconds(0.3f);
        testresult = opponent.IsBigPunchToTheFace();
        yield return new WaitForSeconds(0.5f);
        Assert.IsTrue(testresult);

        yield return new WaitForSeconds(3f);

        player.TryPlayPunch(player.GetJabRight());
        opponent.TryPlayPunch(opponent.GetCrossLeft());
        yield return new WaitForSeconds(0.2f);
        opponent.ReceiveHit(player, "Head", player.lastattack, true);
        yield return new WaitForSeconds(0.3f);
        testresult = opponent.IsBigPunchToTheFace();
        yield return new WaitForSeconds(0.5f);
        Assert.IsTrue(testresult);

        yield return new WaitForSeconds(3f);

        player.TryPlayPunch(player.GetJabRight());
        opponent.TryPlayPunch(opponent.GetUppercutLeft());
        yield return new WaitForSeconds(0.2f);
        opponent.ReceiveHit(player, "Head", player.lastattack, true);
        yield return new WaitForSeconds(0.3f);
        testresult = opponent.IsBigPunchToTheFace();
        yield return new WaitForSeconds(0.5f);
        Assert.IsTrue(testresult);

        yield return new WaitForSeconds(3f);

        player.TryPlayPunch(player.GetJabRight());
        opponent.TryPlayPunch(opponent.GetUppercutRight());
        yield return new WaitForSeconds(0.2f);
        opponent.ReceiveHit(player, "Head", player.lastattack, true);
        yield return new WaitForSeconds(0.3f);
        testresult = opponent.IsBigPunchToTheFace();
        yield return new WaitForSeconds(0.5f);
        Assert.IsTrue(testresult);
    }

    [UnityTest]

    public IEnumerator JabRightNoCritTest()
    {

        player.TryPlayPunch(player.GetJabRight());
        opponent.TryPlayPunch(opponent.GetJabRight());
        yield return new WaitForSeconds(0.2f);
        opponent.ReceiveHit(player, "Head", player.lastattack, false);
        yield return new WaitForSeconds(0.3f);
        bool testresult = opponent.IsSmallPunchToTheFace();
        yield return new WaitForSeconds(0.5f);
        Assert.IsTrue(testresult);

    }

    [UnityTest]

    public IEnumerator JabLeftNoCritTest()
    {

        player.TryPlayPunch(player.GetJabLeft());
        opponent.TryPlayPunch(opponent.GetJabLeft());
        yield return new WaitForSeconds(0.2f);
        opponent.ReceiveHit(player, "Head", player.lastattack, false);
        yield return new WaitForSeconds(0.3f);
        bool testresult = opponent.IsSmallPunchToTheFace();
        yield return new WaitForSeconds(0.5f);
        Assert.IsTrue(testresult);
    }

    [UnityTest]

    public IEnumerator JabLeftCritTest()
    {

        player.TryPlayPunch(player.GetJabLeft());
        opponent.TryPlayPunch(opponent.GetJabLeft());
        yield return new WaitForSeconds(0.2f);
        opponent.ReceiveHit(player, "Head", player.lastattack, true);
        yield return new WaitForSeconds(0.3f);
        bool testresult = opponent.IsBigPunchToTheFace();
        yield return new WaitForSeconds(0.5f);
        Assert.IsTrue(testresult);

    }

    [UnityTest]

    public IEnumerator UppercutRightNoCritTest()
    {

        player.TryPlayPunch(player.GetUppercutRight());
        opponent.TryPlayPunch(opponent.GetUppercutRight());
        yield return new WaitForSeconds(0.2f);
        opponent.ReceiveHit(player, "Head", player.lastattack, false);
        yield return new WaitForSeconds(0.3f);
        bool testresult = opponent.IsSmallUppercut();
        yield return new WaitForSeconds(0.5f);
        Assert.IsTrue(testresult);

    }

    [UnityTest]

    public IEnumerator UppercutRightCritTest()
    {

        player.TryPlayPunch(player.GetUppercutRight());
        opponent.TryPlayPunch(opponent.GetUppercutRight());
        yield return new WaitForSeconds(0.2f);
        opponent.ReceiveHit(player, "Head", player.lastattack, true);
        yield return new WaitForSeconds(0.3f);
        bool testresult = opponent.IsBigUppercut();
        yield return new WaitForSeconds(0.5f);
        Assert.IsTrue(testresult);
    }

    [UnityTest]

    public IEnumerator UppercutLeftNoCritTest()
    {


        player.TryPlayPunch(player.GetUppercutLeft());
        opponent.TryPlayPunch(opponent.GetUppercutLeft());
        yield return new WaitForSeconds(0.2f);
        opponent.ReceiveHit(player, "Head", player.lastattack, false);
        yield return new WaitForSeconds(0.3f);
        bool testresult = opponent.IsSmallUppercut();
        yield return new WaitForSeconds(0.5f);
        Assert.IsTrue(testresult);

    }


    [UnityTest]

    public IEnumerator UppercutLeftCritTest()
    {
        player.TryPlayPunch(player.GetUppercutLeft());
        opponent.TryPlayPunch(opponent.GetUppercutLeft());
        yield return new WaitForSeconds(0.2f);
        opponent.ReceiveHit(player, "Head", player.lastattack, true);
        yield return new WaitForSeconds(0.3f);
        bool testresult = opponent.IsBigUppercut();
        yield return new WaitForSeconds(0.5f);
        Assert.IsTrue(testresult);
    }

    [UnityTest]

    public IEnumerator CrossRightNoCritTest()
    {
        player.TryPlayPunch(player.GetCrossRight());
        opponent.TryPlayPunch(opponent.GetCrossRight());
        yield return new WaitForSeconds(0.2f);
        opponent.ReceiveHit(player, "Head", player.lastattack, false);
        yield return new WaitForSeconds(0.3f);
        bool testresult = opponent.IsSmallRightCross();
        yield return new WaitForSeconds(0.5f);
        Assert.IsTrue(testresult);
    }

    [UnityTest]

    public IEnumerator CrossRightCritTest()
    {
        player.TryPlayPunch(player.GetCrossRight());
        opponent.TryPlayPunch(opponent.GetCrossRight());
        yield return new WaitForSeconds(0.2f);
        opponent.ReceiveHit(player, "Head", player.lastattack, true);
        yield return new WaitForSeconds(0.3f);
        bool testresult = opponent.IsBigRightCross();
        yield return new WaitForSeconds(0.5f);
        Assert.IsTrue(testresult);
    }

    [UnityTest]

    public IEnumerator CrossLeftCritTest() 
    {
        player.TryPlayPunch(player.GetCrossLeft());
        opponent.TryPlayPunch(opponent.GetCrossLeft());
        yield return new WaitForSeconds(0.2f);
        opponent.ReceiveHit(player, "Torso", player.lastattack, true);
        yield return new WaitForSeconds(0.3f);
        bool testresult = opponent.IsLivershotKnockdown();
        yield return new WaitForSeconds(0.5f);
        Assert.IsTrue(testresult);
    }
    [UnityTest]

    public IEnumerator CrossLeftNoCritTest() 
    {
        player.TryPlayPunch(player.GetCrossLeft());
        opponent.TryPlayPunch(opponent.GetCrossLeft());
        yield return new WaitForSeconds(0.2f);
        opponent.ReceiveHit(player, "Torso", player.lastattack, true);
        yield return new WaitForSeconds(0.3f);
        bool testresult = opponent.IsLivershotKnockdown();
        yield return new WaitForSeconds(0.5f);
        Assert.IsTrue(testresult);
    }
}
