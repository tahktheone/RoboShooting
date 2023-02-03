import bpy

def add_anim(of1):    
    l_offset = of1
    
    # Get reference to Follow Path constraint
    ob = bpy.context.active_object
    fpc = [c for c in ob.constraints if c.name == "Follow Path"]    
    fpc[0].offset = l_offset
    bpy.ops.ed.undo_push(message = "Restore point 1")
    bpy.ops.constraint.apply(constraint=fpc[0].name)
    l0 = ob.location.copy()
    r0 = ob.rotation_euler.copy()
    bpy.ops.ed.undo()

    ob = bpy.context.active_object
    fpc = [c for c in ob.constraints if c.name == "Follow Path"]
    l_offset = l_offset + 2.175
    fpc[0].offset = l_offset
    bpy.ops.ed.undo_push(message = "Restore point 2")
    bpy.ops.constraint.apply(constraint=fpc[0].name)
    l1 = ob.location.copy()
    r1 = ob.rotation_euler.copy()
    bpy.ops.ed.undo()
    
    ob = bpy.context.active_object
    fpc = [c for c in ob.constraints if c.name == "Follow Path"]
    l_offset = l_offset + 2.175
    fpc[0].offset = l_offset
    bpy.ops.ed.undo_push(message = "Restore point 3")
    bpy.ops.constraint.apply(constraint=fpc[0].name)
    l2 = ob.location.copy()
    r2 = ob.rotation_euler.copy()
    bpy.ops.ed.undo()
    
    ob = bpy.context.active_object
    fpc = [c for c in ob.constraints if c.name == "Follow Path"]
    l_offset = l_offset + 2.175
    fpc[0].offset = l_offset
    bpy.ops.ed.undo_push(message = "Restore point 4")
    bpy.ops.constraint.apply(constraint=fpc[0].name)
    l3 = ob.location.copy()
    r3 = ob.rotation_euler.copy()
    bpy.ops.ed.undo()     
    
    ob = bpy.context.active_object
    fpc = [c for c in ob.constraints if c.name == "Follow Path"]
    l_offset = l_offset + 2.175
    fpc[0].offset = l_offset
    bpy.ops.ed.undo_push(message = "Restore point 5")
    bpy.ops.constraint.apply(constraint=fpc[0].name)
    l4 = ob.location.copy()
    r4 = ob.rotation_euler.copy()
    #bpy.ops.ed.undo()     

    ob.location = l0
    ob.rotation_euler = r0
    ob.keyframe_insert(data_path="location", frame=0)
    ob.keyframe_insert(data_path="rotation_euler", frame=0)
    
    ob.location = l1
    ob.rotation_euler = r1
    ob.keyframe_insert(data_path="location", frame=25)
    ob.keyframe_insert(data_path="rotation_euler", frame=25)
    
    ob.location = l2
    ob.rotation_euler = r2
    ob.keyframe_insert(data_path="location", frame=50)
    ob.keyframe_insert(data_path="rotation_euler", frame=50)
    
    ob.location = l3
    ob.rotation_euler = r3
    ob.keyframe_insert(data_path="location", frame=75)
    ob.keyframe_insert(data_path="rotation_euler", frame=75)
    
    ob.location = l4
    ob.rotation_euler = r4
    ob.keyframe_insert(data_path="location", frame=100)
    ob.keyframe_insert(data_path="rotation_euler", frame=100)
    
    # Get the animation data of the object
    animation_data = ob.animation_data

    # Loop through all fcurves of the animation data
    for fcurve in animation_data.action.fcurves:
        # Loop through all keyframes of the fcurve
        for keyframe in fcurve.keyframe_points:
            # Change the handle type to 'VECTOR'
            keyframe.handle_left_type = 'VECTOR'
            keyframe.handle_right_type = 'VECTOR'
            
# Get the active object
obj = bpy.context.active_object

for i in range(0, 46):
    new_obj = obj.copy()
    new_obj.data = obj.data.copy()    
    bpy.context.scene.collection.objects.link(new_obj)   
    bpy.context.view_layer.objects.active = new_obj
    bpy.ops.ed.undo_push(message = "Restore point 0")             
    add_anim(i*2.175)
else:
    print("The object does not have a 'Follow Path' constraint.")