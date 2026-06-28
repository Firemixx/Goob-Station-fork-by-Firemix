using System.Linq;
using Content.Shared.Emag.Systems;
using Content.Shared.Popups;
using Content.Shared.Salvage.Fulton;
using Robust.Shared.Audio.Systems;

namespace Content.Goobstation.Shared.Salvage.Fulton;
// Emagged interaction with EMAG
public sealed class EmaggedFultonSystem : EntitySystem
{
    [Dependency] protected readonly SharedAudioSystem Audio = default!;
    [Dependency] private   readonly SharedPopupSystem _popup = default!;
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<FultonComponent, GotEmaggedEvent>(OnEmagged);
    }

    private void OnEmagged(EntityUid ent,FultonComponent comp, ref GotEmaggedEvent args)
    {
        ChangeWhitelistToEvac(comp,"MindContainer"); // All mobs can be extracted by fulton after emagging
        _popup.PopupEntity(Loc.GetString("fulton-emagged"), ent);
        Audio.PlayPredicted(comp.FultonSoundEmag, ent, ent);
    }


    // Adding new Comp to whitelist for evac
    public void ChangeWhitelistToEvac(FultonComponent comp,string nameComp)
    {
        if (comp==null)
            return;
        comp.Whitelist.Components=comp.Whitelist.Components
        .Union(new[] { nameComp })
        .ToArray();
    }
}
