using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;


/// This is in every presentation entity that is controlled/influenced by simulation entities.
public struct C_SimulationOwnerReference : IComponentData {	
	public uint OwnerFrameStateHash;
	public Entity SimulationOwnerEntity;
	public uint CreationSimulationFrame;
}

public struct TC_PresentationEntityPendingConfirmation : IComponentData
{
	
}


public struct CM_CreatePresentationEntity : IComponentData {
	public uint CreationSimulationFrame;
	public uint PresentationEntityConfigId;
	public Entity SimulationOwnerEntity;
	public Vector2 Position;
	
	// Calculated in simulation world using simulation world state. Such as: CharacterPositions, CharacterHealths.
	public uint OwnerFrameStateHash;
	
	// Knows whether or not this message was created in a confirm frame
	public byte FromConfirmFrame;	
	
	
}

// System that runs on the simulation world once per simulation frame and stores maps frames to specific data cache.
// These maps function as Permanent Rolling Windows for each variable that gets taken into account to generate the FrameStateHash.
public class S_GenerateFrameStateDataCaches : JobComponentSystem {
	protected override JobHandle OnUpdate(JobHandle inputDeps)
	{
		throw new System.NotImplementedException();
	}
}

// System that runs on the simulation world once per render frame before sending out the messages.
// It accesses the FrameStateDataCache for each of the messages's using the message frame as an index, generates the FrameStateHash and stores it in each CM_CreatePresentationEntity.
public class S_GenerateFrameStateHashses : JobComponentSystem {
	protected override JobHandle OnUpdate(JobHandle inputDeps)
	{
		throw new System.NotImplementedException();
	}
}


// System that runs in the PresentationWorld that is responsible for spawning all presentation entities.
public class S_SpawnPresentationEntities : JobComponentSystem {

	// Job that, for each CM_CreatePresentationEntity, goes through all C_SimulationOwnerReference. 
	// If there is a match FrameStateHashes, Position and PresentationEntityConfigId enqueues ownership transfers.
	// If there is no match, enqueue a Create Presentation Entity struct. 
	public struct J_VerifyOwnership : IJobChunk {
		public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
		{
			throw new System.NotImplementedException();
		}
	}

	// Job that goes through the enqueued ownership transfers and writes to each C_SimulationOwnerReferece based on it.
	public struct J_EnforceOwnership : IJobParallelFor {
		public void Execute(int index)
		{
			throw new System.NotImplementedException();
		}
	}

	// Job that goes through the enqueued Create Presentation Entity structs and enqueues, in a EntityCommandBuffer, the creation of presentation entities.
	public struct J_CreatePresentationEntity : IJobParallelFor {
		public void Execute(int index)
		{
			throw new System.NotImplementedException();
		}
	}

	protected override JobHandle OnUpdate(JobHandle inputDeps)
	{
		throw new System.NotImplementedException();
	}
}

// System that runs in the Presentation World that is responsible for validating that certain entities are guaranteed to have been created. 
// As well as, ensuring existing entities for which no creation message has been received are disabled. If they are disabled for longer than the rollback window, destoy them.
public class S_ValidatePresentationEntities : JobComponentSystem {
	protected override JobHandle OnUpdate(JobHandle inputDeps)
	{
		throw new System.NotImplementedException();
	}
}